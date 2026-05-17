using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Taskorium.IntegrationEvents.Dto;
using Taskorium.IntegrationEvents.Notifications;
using Taskorium.MessageBus.Abstractions;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Features.Projects.Read.Query;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Services;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueCreateHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    FileStorageService fileStorageService,
    ICurrentUserContext currentUser,
    IEventBus eventBus,
    IDispatcher dispatcher)
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

        IssueAssignees assignee = IssueAssignees.Create(
            userId: currentUser.User.Id,
            issueId: issue.Id,
            role: ProjectRoles.Creator);


        List<Attachment> attachments = new(request.AttachmentDtos?.Count ?? 0);
        try
        {
            if (request.AttachmentDtos != null)
            {
                foreach (var attach in request.AttachmentDtos)
                {
                    Attachment attachment = Attachment.Create(
                        issueId: issue.Id,
                        uploaderId: currentUser.User.Id,
                        fileName: attach.Name,
                        contentType: attach.ContentType,
                        contentLength: attach.ContentLength);

                    //сброс позиции чтения файла
                    if (attach.Content.CanSeek)
                        attach.Content.Position = 0;

                    await fileStorageService.UploadAsync(
                        name: attachment.StoragePath,
                        contentType: attach.ContentType,
                        stream: attach.Content,
                        token: cancellationToken);

                    attachments.Add(attachment);
                    issue.Attachments.Add(attachment);
                }
            }

            context.Issues.Add(issue);
            context.IssueAssignees.Add(assignee);
            await context.SaveChangesAsync(cancellationToken);

            var content = new NotificationEventContent
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

            var membersQuery = new GetProjectMembersQuery(request.ProjectId);
            var membersResult = await dispatcher.SendAsync(membersQuery, cancellationToken);

            var recipients = membersResult.Members
                .Where(m => !string.IsNullOrWhiteSpace(m.Email))
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

            await eventBus.PublishAsync(new NotificationCreatedIntegrationEvent(content, recipients), cancellationToken);
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

        return issue.ToResponse();
    }
}
