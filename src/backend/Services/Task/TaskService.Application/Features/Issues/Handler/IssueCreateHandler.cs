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
        _ = await context.Projects.FindAsync([request.ProjectId], cancellationToken) ??
            throw new KeyNotFoundException($"Проект с id: {request.IssueStatusId} не найдена");

        var status = await context.IssueStatus.FindAsync([request.IssueStatusId], cancellationToken) ??
                     throw new KeyNotFoundException($"Статус задачи с id: {request.IssueStatusId} не найдена");

        //TODO: проверить что можно создавать с этим статусом

        var tag = await context.IssueTag.FindAsync([request.IssueTagId], cancellationToken) ??
                   throw new KeyNotFoundException($"Тип задачи с id: {request.IssueTagId} не найдена");

        //TODO: проверить что можно создавать с этим типом

        var issue = Issue.Create(
            name: request.Name,
            description: request.Description,
            projectId: request.ProjectId,
            taskTagId: request.IssueTagId,
            taskStatusId: request.IssueStatusId,
            numberIssueType: request.numberIssueType,
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
