using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Update;

public class UpdateWorkspaceNameHandler(TaskServiceDbContext context) : IRequestHandler<UpdateWorkspaceNameCommand, UpdateWorkspaceNameResult>
{
    public async Task<UpdateWorkspaceNameResult> Handle(UpdateWorkspaceNameCommand request, CancellationToken cancellationToken = default)
    {
        var workspace = await context.Workspaces.FindAsync(request.Id);
        if (workspace == null)
        {
            throw new ArgumentException();
        }
        workspace.UpdateName(request.Name);
        await context.SaveChangesAsync(cancellationToken);
        return new UpdateWorkspaceNameResult(workspace.Id, workspace.Name.Value);
    }
}
