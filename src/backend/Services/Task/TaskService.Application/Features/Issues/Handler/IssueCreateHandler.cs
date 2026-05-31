using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Taskorium.IntegrationEvents.Dto;
using Taskorium.IntegrationEvents.Notifications;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Features.Projects.Read.GetProjectMembers;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Outbox.Interfaces;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Services;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueCreateHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    FileStorageService fileStorageService,
    ICurrentUserContext currentUser,
    IOutboxMessageFactory outboxMessageFactory,
    IDispatcher dispatcher)
    //IssueNotificationService notificationService)
    : IRequestHandler<IssueCreateCommand, IssueResponse>
{
    public async Task<IssueResponse> Handle(IssueCreateCommand request, CancellationToken cancellationToken = default)
    {
        var project = await context.Projects.FindAsync([request.ProjectId], cancellationToken) ??
                      throw new KeyNotFoundException($"Проект с id: {request.ProjectId} не найдена");

        var status =
            await context.IssueStatus.FirstOrDefaultAsync(
                element => element.ProjectId == request.ProjectId && element.Type == IssueStatusType.Initial,
                cancellationToken) ??
            throw new KeyNotFoundException($"Не найден статус инициализации задачи для проекта {request.ProjectId}");

        //TODO: костыль. возможна гонка. нужно добавить отдельную таблицу счетчиков, которая будет возвращать будущий номер и при этом делать внутри ++
        var countIssue = await context.Issues.CountAsync(x => x.ProjectId == project.Id, cancellationToken);
        var issueKey = $"{project.Abbreviation}-{countIssue + 1}";

        var issue = Issue.Create(
            request.Name,
            request.Description,
            issueKey,
            request.ProjectId,
            status.Id,
            request.NumberIssueType,
            request.NumberIssuePriority,
            request.DueDate
        );

        var assignee = IssueAssignees.Create(
            currentUser.User.Id,
            issue.Id,
            ProjectRoles.Creator);


        List<Attachment> attachments = new(request.AttachmentDtos?.Count ?? 0);
        try
        {
            if (request.AttachmentDtos != null)
            {
                foreach (var attach in request.AttachmentDtos)
                {
                    var attachment = Attachment.Create(
                        issue.Id,
                        currentUser.User.Id,
                        attach.Name,
                        attach.ContentType,
                        attach.ContentLength);

                    //сброс позиции чтения файла
                    if (attach.Content.CanSeek)
                    {
                        attach.Content.Position = 0;
                    }

                    await fileStorageService.UploadAsync(
                        attachment.StoragePath,
                        attach.ContentType,
                        attach.Content,
                        cancellationToken);

                    attachments.Add(attachment);
                    issue.Attachments.Add(attachment);
                }
            }

            context.Issues.Add(issue);
            context.IssueAssignees.Add(assignee);

            var eventContent = new NotificationEventContent
            {
                Subject = $"Создана задача {issueKey}",
                Body = $"Задача '{issue.Name}' успешно создана",
                ActionUrl = $"/projects/{issue.ProjectId}/issues/{issue.Id}",
                Metadata = new Dictionary<string, string>
                {
                    ["issueId"] = issue.Id.ToString(),
                    ["issueKey"] = issueKey,
                    ["projectId"] = issue.ProjectId.ToString(),
                    ["creatorId"] = currentUser.User.Id.ToString()
                }
            };

            //Собрали всех учатсников проекта
            var membersQuery = new GetProjectMembersQuery(request.ProjectId);
            var membersResult = await dispatcher.SendAsync(membersQuery, cancellationToken);

            var recipients = membersResult.Members
                .Where(m => !string.IsNullOrWhiteSpace(m.Email)
                            && m.UserId != currentUser.User.Id)
                //Нужна ли проверка на роли?
                //&& (m.Role == Contracts.Enum.ProjectRolesDto.Admin || m.Role == Contracts.Enum.ProjectRolesDto.Creator))
                .GroupBy(m => m.UserId)
                .Select(g => g.First())
                .Select(m => new NotificationRecipient
                {
                    UserId = m.UserId.ToString(),
                    FullName = m.UserName ?? "",
                    Email = m.Email
                })
                .Where(e => e.Email != null)
                .ToList();

            //Создали событие для уведомления участников проекта о создании новой задачи
            var integrationEvent = new NotificationCreatedIntegrationEvent(eventContent, recipients);
            //TODO: вынести создание события в отдельный сервис, который будет принимать тип события и контент, а дальше уже внутри определять как создавать событие и кому отправлять
            var outboxMessage = outboxMessageFactory.Create(integrationEvent, nameof(NotificationCreatedIntegrationEvent));

            context.OutboxMessages.Add(outboxMessage);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (attachments.Count > 0)
            {
                var tasks = attachments.Select(async delete =>
                {
                    try
                    {
                        await fileStorageService.DeleteAsync(delete.StoragePath, cancellationToken);
                    }
                    catch
                    {
                        //TODO: logger
                    }
                });
                await Task.WhenAll(tasks);
            }

            throw;
        }

        try
        {
            var cacheKey = $"issues_by_project_id_{request.ProjectId}";
            await cache.RemoveAsync(cacheKey, cancellationToken);
        }
        catch
        {
            //TODO: logger
        }

        //Отправка в нотификатор напрямую
        //try
        //{
        //    //TODO cache
        //    // Получаем Keycloak ID участников проекта с правами Creator/Admin (руководители).
        //    var managerKeycloakIds = await context.ProjectMembers
        //        .Where(pm => pm.ProjectId == request.ProjectId
        //                     && !pm.IsDeleted
        //                     && (pm.Role == ProjectRoles.Creator || pm.Role == ProjectRoles.Admin))
        //        .Join(context.Users,
        //            pm => pm.UserId,
        //            u => u.Id,
        //            (pm, u) => u.KeycloakId)
        //        .ToListAsync(cancellationToken);

        //    // Исполнитель = текущий пользователь (создатель задачи).
        //    // Объединяем с менеджерами и убираем дубликаты.
        //    var recipientKeycloakIds = managerKeycloakIds
        //        .Append(currentUser.User.KeycloakId)
        //        .Distinct()
        //        .Select(id => id.ToString());

        //    await notificationService.NotifyIssueCreatedAsync(
        //        issue.Id,
        //        issue.Name.ToString(),
        //        issue.Key.Value,
        //        project.Id,
        //        project.Name.ToString(),
        //        recipientKeycloakIds,
        //        cancellationToken);
        //}
        //catch
        //{
        //    //TODO: logger — не прерываем основной поток при сбое уведомления
        //}

        return issue.ToResponse();
    }
}
