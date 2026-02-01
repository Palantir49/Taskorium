
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Update
{
    public record UpdateWorkspaceNameCommand(Guid Id, string Name):ICommand<UpdateWorkspaceNameResult>;
    
}
