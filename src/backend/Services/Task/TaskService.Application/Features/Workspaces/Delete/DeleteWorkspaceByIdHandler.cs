using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Delete;

public class DeleteWorkspaceByIdHandler(TaskServiceDbContext context, HybridCache cache) : IRequestHandler<DeleteWorkspaceByIdCommand, DeleteWorkspaceByIdResult>
{
    public async Task<DeleteWorkspaceByIdResult> Handle(DeleteWorkspaceByIdCommand request, CancellationToken cancellationToken = default)
    {
        var workspace = await context.Workspaces.FindAsync(request.Id);
        if (workspace == null)
        {
            throw new ArgumentException();
        }
        context.Workspaces.Remove(workspace);
        await context.SaveChangesAsync(cancellationToken);
        var cacheKey = $"workspace_{request.Id}";
        await cache.RemoveAsync(cacheKey, cancellationToken);
        return new DeleteWorkspaceByIdResult(workspace.Id, workspace.Name.Value);
    }
}

