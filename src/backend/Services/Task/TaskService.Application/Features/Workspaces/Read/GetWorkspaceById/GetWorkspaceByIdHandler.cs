using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Read.GetWorkspaceById;

public class GetWorkspaceByIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<GetWorkspaceByIdQuery, GetWorkspacebyIdResult>
{
    public async Task<GetWorkspacebyIdResult> Handle(GetWorkspaceByIdQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = $"workspace_{query.Id}";

        return await cache.GetOrCreateAsync(cacheKey,
            async ct => await GetWorkspaceFromDbAsync(query.Id, ct),
            cancellationToken: cancellationToken);
    }

    public async Task<GetWorkspacebyIdResult> GetWorkspaceFromDbAsync(Guid workspaceId, CancellationToken cancellationToken)
    {
        var workspace = await context.Workspaces.FindAsync([workspaceId], cancellationToken);
        if (workspace == null)
        {
            throw new KeyNotFoundException($"Рабочая область с id: {workspaceId} не найдена");
        }

        return new GetWorkspacebyIdResult(
            workspace.Id,
            workspace.Name.ToString());
    }
}
