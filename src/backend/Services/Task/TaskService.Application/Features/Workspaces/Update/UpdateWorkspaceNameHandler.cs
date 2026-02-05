using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Workspaces.Update;

public class UpdateWorkspaceNameHandler(IRepositoryWrapper wrapper) : IRequestHandler<UpdateWorkspaceNameCommand, UpdateWorkspaceNameResult>
{
    public async Task<UpdateWorkspaceNameResult> Handle(UpdateWorkspaceNameCommand request, CancellationToken cancellationToken = default)
    {
        var workspace = await wrapper.Workspaces.GetByIdAsync(request.Id);
        if (workspace == null)
        {
            throw new ArgumentException();
        }
        workspace.UpdateName(request.Name);
        await wrapper.SaveChangesAsync();
        return new UpdateWorkspaceNameResult(workspace.Id, workspace.Name.Value);
    }
}
