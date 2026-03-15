using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueUpdateHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<IssueUpdateCommand, IssueResponse>
{
    public async Task<IssueResponse> Handle(IssueUpdateCommand request, CancellationToken cancellationToken = default)
    {
        var issue = await context.Issues.FindAsync([request.id], cancellationToken) ??
                    throw new NullReferenceException($"Задача с id: {request.id} не найдена");

        var project = await context.Projects.FindAsync([issue.ProjectId], cancellationToken);
        if (project is null)
        {
            throw new InvalidOperationException($"Проект с id: {issue.ProjectId}, связанный с задачей, не существует");
        }

        var status = await context.IssueStatus.FindAsync([request.IssueStatusId], cancellationToken) ??
                     throw new NullReferenceException($"Статус задачи с id: {request.IssueStatusId} не найден");


        issue.UpdateName(request.Name);
        issue.UpdateDescription(request.Description);
        issue.UpdateType(request.numberIssueType);
        issue.UpdateStatus(status);
        issue.UpdateDueDate(request.DueDate);

        context.Issues.Update(issue);
        await context.SaveChangesAsync(cancellationToken);

        //Инвалидируем кэш:
        var issueCacheKey = $"issue_id_{issue.Id}";
        // 2. Список задач проекта
        var projectIssuesCacheKey = $"issues_by_project_id_{issue.ProjectId}";


        await cache.RemoveAsync(issueCacheKey, cancellationToken);
        await cache.RemoveAsync(projectIssuesCacheKey, cancellationToken);


        return issue.ToResponse();
    }
}
