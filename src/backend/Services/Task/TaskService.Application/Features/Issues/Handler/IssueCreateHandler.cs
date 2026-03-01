using Microsoft.EntityFrameworkCore;
using TaskService.Application.Commands.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueCreateHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<IssueCreateCommand, IssueResponse>
{
    public async Task<IssueResponse> Handle(IssueCreateCommand request, CancellationToken cancellationToken = default)
    {
        var project = await context.Projects.FindAsync([request.ProjectId], cancellationToken) ??
            throw new KeyNotFoundException($"Проект с id: {request.IssueStatusId} не найдена");

        var status = await context.IssueStatus.FindAsync([request.IssueStatusId], cancellationToken) ??
                     throw new KeyNotFoundException($"Статус задачи с id: {request.IssueStatusId} не найдена");

        int countIssue = await context.Issues.CountAsync(x => x.ProjectId == project.Id, cancellationToken);

        string issueKey = $"{project.Abbreviation}-{countIssue + 1}";

        var issue = Issue.Create(
            name: request.Name,
            description: request.Description,
            key: issueKey,
            projectId: request.ProjectId,
            taskTagId: request.IssueTagId,
            taskStatusId: status.Id,
            numberIssueType: request.NumberIssueType,
            numberIssuePriority: request.NumberIssuePriority,
            dueDate: request.DueDate
        );
        await context.Issues.AddAsync(issue, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидация кэша
        var cacheKey = $"issues_by_project_id_{request.ProjectId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return issue.ToResponse();
    }
}
