using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Workspaces.Write.Command;
using TaskService.Application.Features.Workspaces.Write.Result;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Write.Handler;

public class DeleteWorkspaceByIdHandler(TaskServiceDbContext context, HybridCache cache) : IRequestHandler<DeleteWorkspaceByIdCommand, DeleteWorkspaceByIdResult>
{
    public async Task<DeleteWorkspaceByIdResult> Handle(DeleteWorkspaceByIdCommand request, CancellationToken cancellationToken = default)
    {
        var workspace = await context.Workspaces.Include(w => w.WorkspaceMembers)
                                                .Include(w => w.Projects)
                                                .ThenInclude(p => p.ProjectMembers)
                                                .AsSplitQuery()
                                                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
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

