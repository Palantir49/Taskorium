using TaskService.Application.Features.Workspaces.Write.CreateWorkspace;
using TaskService.Contracts.Workspace.Request;

namespace TaskService.Application.Commands.Workspaces;

public static class WorkspaceMapping
{
    //public static GetWorkspaceQuery ToCommand(this GetWorkspaceRequest request)
    //{
    //    return new GetWorkspaceQuery(Name: request.Name, ownerId: request.OwnerId);
    //}
    public static CreateWorkspaceCommand ToCommand(this CreateWorkspaceRequest request)
    {
        return new CreateWorkspaceCommand(Name: request.Name, OwnerId: request.OwnerId);
    }
    //TODO: UpdateCommand
    //TODO: DeleteCommand
    //TODO: ReadCommand
}
