using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Commands.Workspaces.Create;

public class CreateWorkspaceHandler(TaskServiceDbContext context)
    : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
{
    public async Task<CreateWorkspaceResult> Handle(CreateWorkspaceCommand command,
        CancellationToken cancellationToken = default)
    {
        var workspace = Workspace.Create(
            command.Name
        );
        await context.Workspaces.AddAsync(workspace, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);


        return new CreateWorkspaceResult(
            workspace.Id,
            workspace.Name.ToString());
    }
}
