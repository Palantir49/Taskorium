using TaskService.Application.Mediator;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Workspaces.Create;

public class CreateWorkspaceHandler : ICommandHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
{
    private readonly IRepositoryWrapper _wrapper;

    public CreateWorkspaceHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<CreateWorkspaceResult> Handle(CreateWorkspaceCommand command, CancellationToken cancellationToken = default)
    {
        var workspace = Workspace.Create(
            name: command.Name,
            ownerId: command.ownerId
        );
        await _wrapper.Workspaces.AddAsync(workspace, cancellationToken);
        await _wrapper.SaveChangesAsync(cancellationToken);

        return new CreateWorkspaceResult(
            id: workspace.Id,
            name: workspace.Name.ToString());
    }


}
