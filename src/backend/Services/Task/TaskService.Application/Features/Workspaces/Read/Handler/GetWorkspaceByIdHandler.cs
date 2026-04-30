using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Workspaces.Read.Query;
using TaskService.Application.Features.Workspaces.Read.Result;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Read.Handler;

public class GetWorkspaceHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<GetWorkspaceByIdQuery, GetWorkspacebyIdResult>
{
    public async Task<GetWorkspacebyIdResult> Handle(GetWorkspaceByIdQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = $"workspace_{query.Id}";

        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var workspace = await context.Workspaces.FindAsync([query.Id], cancellationToken);
            if (workspace == null)
            {
                throw new KeyNotFoundException($"Рабочая область с id: {query.Id} не найдена");
            }

            return new GetWorkspacebyIdResult(
                workspace.Id,
                workspace.Name.ToString());
        }, cancellationToken: cancellationToken);
    }
}
