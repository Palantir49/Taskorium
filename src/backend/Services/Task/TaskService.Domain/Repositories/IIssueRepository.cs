using TaskService.Domain.Entities;
using TaskService.Domain.IRepositories;

namespace TaskService.Domain.Repositories;

public interface IIssueRepository : IRepositoryBase<Issue>
{
    Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
}
