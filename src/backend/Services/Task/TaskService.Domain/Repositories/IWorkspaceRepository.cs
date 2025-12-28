using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories;

public interface IWorkspaceRepository
{
    Task AddAsync(Workspace workspace, CancellationToken ct = default);
    Task<Workspace?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Workspace>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
