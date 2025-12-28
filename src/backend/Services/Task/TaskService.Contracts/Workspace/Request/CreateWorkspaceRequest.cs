
namespace TaskService.Contracts.Workspace.Request
{
    public record class CreateWorkspaceRequest(string Name, Guid? ownerId = null);
}
