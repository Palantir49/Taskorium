using TaskService.Domain.Entities;

namespace TaskService.Domain.IRepositories
{
    public interface ITaskRepository
    {
        Task AddAsync(Issue issue, CancellationToken ct = default);
        Task<Issue?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
