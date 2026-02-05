using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Workspaces.Delete;

public class DeleteWorkspaceByIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<DeleteWorkspaceByIdCommand, DeleteWorkspaceByIdResult>
{
    public async Task<DeleteWorkspaceByIdResult> Handle(DeleteWorkspaceByIdCommand request, CancellationToken cancellationToken = default)
    {
        var workspace = await wrapper.Workspaces.GetByIdAsync(request.Id);
        if (workspace == null)
        {
            throw new ArgumentException();
        }
        await wrapper.Workspaces.DeleteAsync(workspace);
        await wrapper.SaveChangesAsync();
        return new DeleteWorkspaceByIdResult(workspace.Id, workspace.Name.Value);
    }
}

