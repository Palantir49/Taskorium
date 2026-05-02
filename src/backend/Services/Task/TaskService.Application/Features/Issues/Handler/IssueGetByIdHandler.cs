using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueGetByIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<IssueGetByIdQuery, IssueResponse>
{
    public async Task<IssueResponse> Handle(IssueGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"issue_id_{request.id}";

        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var issue = await context.Issues.Include(x=>x.Attachments)
            .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken)
            ?? throw new KeyNotFoundException($"задача с id: {request.id} не найдена");

            return issue.ToResponse();
        }, cancellationToken: cancellationToken);
    }
}
