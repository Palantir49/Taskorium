using TaskService.Application.Mediator;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Commands.Workspaces.Create;

public class CreateWorkspaceHandler(TaskServiceDbContext context) : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
{
    public async Task<CreateWorkspaceResult> Handle(CreateWorkspaceCommand command, CancellationToken cancellationToken = default)
    {
        var workspace = Workspace.Create(
            name: command.Name,
            ownerId: command.ownerId
        );
        await context.Workspaces.AddAsync(workspace, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateWorkspaceResult(
            id: workspace.Id,
            name: workspace.Name.ToString());
    }


}
