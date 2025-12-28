using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories;

public interface IProjectRepository
{
    Task AddAsync(Project project, CancellationToken ct = default);
    Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Project>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
