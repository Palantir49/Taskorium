using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories;

public interface IProjectRepository : IRepositoryBase<Project>
{
    Task<List<Project>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken ct = default);
}
