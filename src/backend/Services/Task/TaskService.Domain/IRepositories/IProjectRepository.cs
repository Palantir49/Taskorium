using TaskService.Domain.Entities;

namespace TaskService.Domain.IRepositories
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project, CancellationToken ct = default);
        Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Project>> GetByWorkspaceIdAsync(Guid WorkspaceId, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
