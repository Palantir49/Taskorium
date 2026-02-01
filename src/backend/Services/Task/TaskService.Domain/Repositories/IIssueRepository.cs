using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories;

public interface IIssueRepository : IRepositoryBase<Issue>
{
    Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task<List<Issue>> GetByIssueStatusIdAsync(Guid statusId, CancellationToken ct = default);
    Task<List<Issue>> GetByIssueTypeIdAsync(Guid typeId, CancellationToken ct = default);
}
