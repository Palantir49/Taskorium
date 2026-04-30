using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueCreateHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    //FileStorageService fileStorageService,
    ICurrentUserContext currentUser)
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

        IssueAssignees assignee = IssueAssignees.Create(currentUser.User.Id, issue.Id, Roles.Creator);

        //TODO upload attachments if are not null or empty via fileStorageService

        await context.Issues.AddAsync(issue, cancellationToken);
        await context.IssueAssignees.AddAsync(assignee, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидация кэша
        var cacheKey = $"issues_by_project_id_{request.ProjectId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return issue.ToResponse();
    }
}
