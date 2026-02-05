using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Delete;

public class DeleteWorkspaceByIdHandler(TaskServiceDbContext context) : IRequestHandler<DeleteWorkspaceByIdCommand, DeleteWorkspaceByIdResult>
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
        return new DeleteWorkspaceByIdResult(workspace.Id, workspace.Name.Value);
    }
}

