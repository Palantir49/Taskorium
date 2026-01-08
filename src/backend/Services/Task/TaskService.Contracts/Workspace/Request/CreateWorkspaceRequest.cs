
namespace TaskService.Contracts.Workspace.Request
{
    public record class CreateWorkspaceRequest(string Name, Guid? OwnerId = null);
}
