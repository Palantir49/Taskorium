using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories
{
    public interface IIssueStatusRepository : IRepositoryBase<IssueStatus>
    {
        Task<List<IssueStatus>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    }
}
