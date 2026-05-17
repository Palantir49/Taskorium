using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueDeleteByIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<IssueDeleteByIdCommand, int>
{
    public async Task<int> Handle(IssueDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        var issue = await context.Issues.FindAsync([request.id], cancellationToken) ??
                    throw new KeyNotFoundException($"Задача с id: {request.id} не найдена");

        var projectId = issue.ProjectId;

        context.Issues.Remove(issue);
        var deletedCount = await context.SaveChangesAsync(cancellationToken);

        // Инвалидируем кэш:
        var issueCacheKey = $"issue_id_{issue.Id}";
        var projectIssuesCacheKey = $"issues_by_project_id_{projectId}";


        await cache.RemoveAsync(issueCacheKey, cancellationToken);
        await cache.RemoveAsync(projectIssuesCacheKey, cancellationToken);


        return deletedCount;
    }
}
