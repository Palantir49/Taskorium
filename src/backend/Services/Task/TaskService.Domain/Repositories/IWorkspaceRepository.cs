using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories;

public interface IWorkspaceRepository : IRepositoryBase<Workspace>
{
    Task<List<Workspace>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default);
}
