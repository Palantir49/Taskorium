using TaskService.Contracts.Workspace.Request;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Application.Interfaces
{
    public interface IWorkspaceService
    {
        Task<WorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request, CancellationToken ct = default);
        Task<WorkspaceResponse?> GetWorkspaceByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<WorkspaceResponse>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default);
    }
}
