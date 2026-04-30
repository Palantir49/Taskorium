using TaskService.Application.Features.Workspaces.Write.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Write.Command
{
    public record UpdateWorkspaceNameCommand(Guid Id, string Name) : ICommand<UpdateWorkspaceNameResult>;

}
