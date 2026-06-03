using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Taskorium.IntegrationEvents.Dto;
using Taskorium.IntegrationEvents.Notifications;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Interfaces;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Outbox.Interfaces;
using TaskService.Infrastructure.Outbox.Models;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Services;

namespace TaskService.Application.Features.Issues.Handler;

/// <summary>
///     Обрабатывает команду создания задачи.
///     Оркестрирует: создание доменной сущности, загрузку вложений,
///     сохранение в БД, инвалидацию кэша и отправку уведомлений.
/// </summary>
public sealed class IssueCreateHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    FileStorageService fileStorageService,
    ICurrentUserContext currentUser,
    IOutboxMessageFactory outboxMessageFactory,
    IssueNotificationService notificationService,
    ILogger<IssueCreateHandler> logger,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<IssueCreateCommand, IssueResponse>
{
    public async Task<IssueResponse> Handle(
        IssueCreateCommand request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Начало создания задачи для проекта {ProjectId}", request.ProjectId);

        var (project, initialStatus) = await LoadProjectDataAsync(request.ProjectId, cancellationToken);
        var issueKey = await GenerateIssueKeyAsync(project, cancellationToken);
        var issue = BuildIssue(request, issueKey, initialStatus.Id);

        ValidateDueDate(issue);

        // Единый список получателей — используется и для email (outbox), и для push (SignalR)
        var recipients = await ResolveRecipientsAsync(request, cancellationToken);

        await PersistIssueAsync(request, issue, issueKey, recipients, cancellationToken);

        logger.LogInformation("Задача {IssueKey} успешно создана", issueKey);

        await InvalidateCacheAsync(request.ProjectId, cancellationToken);
        await SendPushNotificationsAsync(issue, project, recipients, cancellationToken);

        return issue.ToResponse();
    }

    // -------------------------------------------------------------------------
    // Загрузка данных
    // -------------------------------------------------------------------------

    private async Task<(Project project, IssueStatus initialStatus)> LoadProjectDataAsync(
        Guid projectId,
        CancellationToken ct)
    {
        var project = await context.Projects.FindAsync([projectId], ct)
                      ?? throw new KeyNotFoundException($"Проект с id {projectId} не найден");

        var initialStatus = await context.IssueStatus
                                .FirstOrDefaultAsync(s => s.ProjectId == projectId && s.Type == IssueStatusType.Initial,
                                    ct)
                            ?? throw new KeyNotFoundException(
                                $"Статус инициализации задачи для проекта {projectId} не найден");

        return (project, initialStatus);
    }

    // -------------------------------------------------------------------------
    // Генерация ключа задачи
    // -------------------------------------------------------------------------

    /// <remarks>
    ///     TODO: заменить на атомарный счётчик в отдельной таблице во избежание гонки
    ///     при параллельном создании задач в одном проекте.
    /// </remarks>
    private async Task<string> GenerateIssueKeyAsync(Project project, CancellationToken ct)
    {
        var count = await context.Issues.CountAsync(i => i.ProjectId == project.Id, ct);
        return $"{project.Abbreviation}-{count + 1}";
    }

    // -------------------------------------------------------------------------
    // Построение доменной сущности
    // -------------------------------------------------------------------------

    private static Issue BuildIssue(IssueCreateCommand request, string issueKey, Guid statusId)
    {
        return Issue.Create(
            request.Name,
            request.Description,
            issueKey,
            request.ProjectId,
            statusId,
            request.NumberIssueType,
            request.NumberIssuePriority,
            request.DueDate);
    }

    // -------------------------------------------------------------------------
    // Валидация
    // -------------------------------------------------------------------------

    private static void ValidateDueDate(Issue issue)
    {
        if (issue.DueDate.HasValue && issue.DueDate < issue.CreatedDate)
        {
            throw new ValidationException("Дата выполнения не может быть раньше даты создания");
        }
    }

    // -------------------------------------------------------------------------
    // Единый список получателей (email + push)
    // -------------------------------------------------------------------------

    /// <summary>
    ///     Загружает из БД менеджеров проекта (Creator/Admin) и исполнителей задачи,
    ///     исключая самого создателя задачи.
    ///     Результат используется как для email-уведомлений (outbox), так и для push (SignalR).
    /// </summary>
    private async Task<List<IssueRecipient>> ResolveRecipientsAsync(
        IssueCreateCommand request,
        CancellationToken ct)
    {
        var assigneeUserIds = request.AssigneeDtos is { Count: > 0 }
            ? request.AssigneeDtos.Select(a => a.UserId).ToHashSet()
            : [];

        var rawRecipients = await context.ProjectMembers
            .Where(pm => pm.ProjectId == request.ProjectId
                         && !pm.IsDeleted
                         && (pm.Role == ProjectRoles.Creator
                             || pm.Role == ProjectRoles.Admin
                             || assigneeUserIds.Contains(pm.UserId)))
            .Join(
                context.Users.Where(u => !u.IsDeleted),
                pm => pm.UserId,
                u => u.Id,
                (pm, u) => new { u.KeycloakId, UserName = u.FullName, u.Email })
            .Where(x => x.KeycloakId != currentUser.User.KeycloakId)
            .ToListAsync(ct);

        return
        [
            .. rawRecipients
                .Where(x => !string.IsNullOrWhiteSpace(x.Email.Value))
                .Select(x => new IssueRecipient
                {
                    KeycloakId = x.KeycloakId, UserName = x.UserName, Email = x.Email.Value
                })
                .Distinct()
        ];
    }

    // -------------------------------------------------------------------------
    // Сохранение: вложения + исполнители + задача + outbox
    // -------------------------------------------------------------------------

    private async Task PersistIssueAsync(
        IssueCreateCommand request,
        Issue issue,
        string issueKey,
        List<IssueRecipient> recipients,
        CancellationToken ct)
    {
        var uploadedAttachments = new List<Attachment>(request.AttachmentDtos?.Count ?? 0);

        try
        {
            await UploadAttachmentsAsync(request, issue, uploadedAttachments, ct);
            AddAssignees(request, issue);

            context.Issues.Add(issue);
            context.OutboxMessages.Add(BuildNotificationOutboxMessage(issue, issueKey, recipients));

            await context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании задачи {IssueKey}", issueKey);
            await RollbackAttachmentsAsync(uploadedAttachments, ct);
            throw;
        }
    }

    // -------------------------------------------------------------------------
    // Вложения
    // -------------------------------------------------------------------------

    private async Task UploadAttachmentsAsync(
        IssueCreateCommand request,
        Issue issue,
        List<Attachment> uploaded,
        CancellationToken ct)
    {
        if (request.AttachmentDtos is not { Count: > 0 })
        {
            return;
        }

        foreach (var dto in request.AttachmentDtos)
        {
            var attachment = Attachment.Create(
                issue.Id,
                currentUser.User.Id,
                dto.Name,
                dto.ContentType,
                dto.ContentLength);

            if (dto.Content.CanSeek)
            {
                dto.Content.Position = 0;
            }

            await fileStorageService.UploadAsync(
                attachment.StoragePath,
                dto.ContentType,
                dto.Content,
                ct);

            uploaded.Add(attachment);
            issue.Attachments.Add(attachment);
        }
    }

    private async Task RollbackAttachmentsAsync(List<Attachment> attachments, CancellationToken ct)
    {
        if (attachments.Count == 0)
        {
            return;
        }

        var deleteTasks = attachments.Select(async a =>
        {
            try
            {
                await fileStorageService.DeleteAsync(a.StoragePath, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Не удалось удалить вложение {StoragePath} при откате", a.StoragePath);
            }
        });

        await Task.WhenAll(deleteTasks);
    }

    // -------------------------------------------------------------------------
    // Исполнители
    // -------------------------------------------------------------------------

    private void AddAssignees(IssueCreateCommand request, Issue issue)
    {
        if (request.AssigneeDtos is not { Count: > 0 })
        {
            return;
        }

        foreach (var assignee in request.AssigneeDtos.Select(dto => IssueAssignees.Create(
                     dto.UserId,
                     issue.Id,
                     dto.ProjectRolesDto.ToEntity())))
        {
            context.IssueAssignees.Add(assignee);
        }
    }

    // -------------------------------------------------------------------------
    // Outbox-сообщение (email через очередь)
    // -------------------------------------------------------------------------

    private OutboxMessage BuildNotificationOutboxMessage(
        Issue issue,
        string issueKey,
        List<IssueRecipient> recipients)
    {
        var clientUrl = GetClientUrl(httpContextAccessor);
        var eventContent = new NotificationEventContent
        {
            Subject = $"Создана задача {issueKey}",
            Body = $"Задача '{issue.Name}' успешно создана",
            ActionUrl = clientUrl,
            Metadata = new Dictionary<string, string>
            {
                ["issueId"] = issue.Id.ToString(),
                ["issueKey"] = issueKey,
                ["projectId"] = issue.ProjectId.ToString(),
                ["creatorId"] = currentUser.User.KeycloakId.ToString()
            }
        };

        // KeycloakId используется как UserId в NotificationRecipient,
        var notificationRecipients = recipients
            .Select(r => new NotificationRecipient
            {
                UserId = r.KeycloakId.ToString(), FullName = r.UserName, Email = r.Email
            })
            .ToList();

        var integrationEvent = new NotificationCreatedIntegrationEvent(eventContent, notificationRecipients);
        return outboxMessageFactory.Create(integrationEvent, nameof(NotificationCreatedIntegrationEvent));
    }

    // -------------------------------------------------------------------------
    // Инвалидация кэша
    // -------------------------------------------------------------------------

    private async Task InvalidateCacheAsync(Guid projectId, CancellationToken ct)
    {
        try
        {
            await cache.RemoveAsync($"issues_by_project_id_{projectId}", ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка инвалидации кэша для проекта {ProjectId}", projectId);
        }
    }

    // -------------------------------------------------------------------------
    // Push-уведомления (SignalR)
    // -------------------------------------------------------------------------

    private async Task SendPushNotificationsAsync(
        Issue issue,
        Project project,
        List<IssueRecipient> recipients,
        CancellationToken ct)
    {
        try
        {
            var keycloakIds = recipients.Select(r => r.KeycloakId.ToString());

            await notificationService.NotifyIssueCreatedAsync(
                issue.Id,
                issue.Name.ToString(),
                issue.Key.Value,
                project.Id,
                project.Name.ToString(),
                keycloakIds,
                ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка отправки push-уведомления для задачи {IssueId}", issue.Id);
        }
    }

    private static string GetClientUrl(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new ArgumentException("Не удалось получить контекст запроса");
        }

        var request = context.Request;
        if (!request.Headers.TryGetValue("Referer", out var referer))
        {
            throw new ArgumentException("Не удалось получить URL клиента из запроса");
        }

        if (string.IsNullOrWhiteSpace(referer))
        {
            throw new ArgumentNullException(nameof(referer));
        }

        return referer.ToString();
    }


    private record IssueRecipient
    {
        internal Guid KeycloakId { get; init; }
        internal required string UserName { get; init; }
        internal string? Email { get; init; }
    }
}
