using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Write.UpdateWorkspaceName
{
    public record UpdateWorkspaceNameCommand(Guid Id, string Name) : ICommand<UpdateWorkspaceNameResult>;

}
