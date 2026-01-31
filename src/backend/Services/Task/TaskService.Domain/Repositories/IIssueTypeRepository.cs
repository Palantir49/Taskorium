using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories
{
    public interface IIssueTypeRepository : IRepositoryBase<IssueType>
    {
        //TODO: Это стало появляется у многих сущностей, может выделить отдельный интерфейс один раз для этого метода?
        //или как вариант сделать еще и расширенный base репозиторий для таких сущностей
        Task<List<IssueType>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    }
}
