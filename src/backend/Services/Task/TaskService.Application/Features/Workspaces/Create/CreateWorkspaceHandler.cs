using TaskService.Application.Mediator;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Workspaces.Create;

public class CreateWorkspaceHandler(IRepositoryWrapper wrapper) : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
{
    public async Task<CreateWorkspaceResult> Handle(CreateWorkspaceCommand command, CancellationToken cancellationToken = default)
    {
        var workspace = Workspace.Create(
            name: command.Name,
            ownerId: command.ownerId
        );
        await wrapper.Workspaces.AddAsync(workspace, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);

        return new CreateWorkspaceResult(
            id: workspace.Id,
            name: workspace.Name.ToString());
    }


}
