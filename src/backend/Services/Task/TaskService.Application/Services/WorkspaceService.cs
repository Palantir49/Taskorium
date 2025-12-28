using TaskService.Application.Interfaces;
using TaskService.Contracts.Workspace.Request;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Domain.IRepositories;

namespace TaskService.Application.Services;

public class WorkspaceService(IWorkspaceRepository workspaceRepository) : IWorkspaceService
{
    public async Task<WorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request,
        CancellationToken ct = default)
    {
        var issue = Workspace.Create(request.Name);
        await workspaceRepository.AddAsync(issue, ct);
        await workspaceRepository.SaveChangesAsync(ct);
        return new WorkspaceResponse(issue.Id, issue.Name, issue.CreatedDate, issue.OwnerId);
    }

    public async Task<List<WorkspaceResponse>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default)
    {
        var result = await workspaceRepository.GetByOwnerIdAsync(ownerId, ct);
        return result.Select(w => new WorkspaceResponse(w.Id, w.Name, w.CreatedDate, w.OwnerId)).ToList();
    }

    public async Task<WorkspaceResponse?> GetWorkspaceByIdAsync(Guid id, CancellationToken ct = default)
    {
        var workspace = await workspaceRepository.GetByIdAsync(id, ct);

        if (workspace == null)
        {
            return null;
        }

        return new WorkspaceResponse(workspace.Id, workspace.Name, workspace.CreatedDate, workspace.OwnerId);
    }
}
