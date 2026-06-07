using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueGetByProjectIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<IssueGetByProjectIdQuery, IEnumerable<IssueResponse>>
{
    public async Task<IEnumerable<IssueResponse>> Handle(IssueGetByProjectIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"issues_by_project_id_{request.projectId}";

        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var issues = await context.Issues
                .Where(x => x.ProjectId == request.projectId)
                .Include(x => x.IssueAssignees)
                .ThenInclude(x => x.User)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return issues.Select(x => x.ToResponse());
        }, cancellationToken: cancellationToken);
    }
}
