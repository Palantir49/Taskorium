using TaskService.Domain.Repositories;

namespace TaskService.Application.Wrapper
{
    public interface IRepositoryWrapper
    {
        IIssueRepository Issues { get; }
        IProjectRepository Projects { get; }
        IWorkspaceRepository Workspaces { get; }
        IUnitOfWork UnitOfWork { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
