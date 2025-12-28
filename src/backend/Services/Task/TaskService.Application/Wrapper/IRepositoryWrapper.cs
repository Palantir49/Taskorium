using TaskService.Domain.Repositories;

namespace TaskService.Application.Wrapper
{
    internal interface IRepositoryWrapper
    {
        IIssueRepository Issues { get; }
        IProjectRepository Projects { get; }
        IWorkspaceRepository Workspaces { get; }
        IUnitOfWork UnitOfWork { get; }

        void SaveChangesAsync(CancellationToken ct = default);
    }
}
