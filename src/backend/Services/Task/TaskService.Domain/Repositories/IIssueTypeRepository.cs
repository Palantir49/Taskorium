using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories
{
    public interface IIssueTypeRepository : IRepositoryBase<IssueType>
    {
        //TODO: Это стало появляется у многих сущностей, может выделить отдельный интерфейс один раз для этого метода?
        Task<List<IssueStatus>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    }
}
