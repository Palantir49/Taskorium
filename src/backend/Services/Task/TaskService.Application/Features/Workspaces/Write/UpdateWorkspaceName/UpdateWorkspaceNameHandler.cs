using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Write.UpdateWorkspaceName;

public class UpdateWorkspaceNameHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<UpdateWorkspaceNameCommand, UpdateWorkspaceNameResult>
{
    public async Task<UpdateWorkspaceNameResult> Handle(UpdateWorkspaceNameCommand request,
        CancellationToken cancellationToken = default)
    {
        var workspace = await context.Workspaces.FindAsync([request.Id], cancellationToken);
        if (workspace == null)
        {
            throw new KeyNotFoundException($"Рабочая область с id: {request.Id} не найдена");
        }

        workspace.UpdateName(request.Name);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var cacheKey = $"workspace_{request.Id}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return new UpdateWorkspaceNameResult(workspace.Id, workspace.Name.Value);
    }
}
