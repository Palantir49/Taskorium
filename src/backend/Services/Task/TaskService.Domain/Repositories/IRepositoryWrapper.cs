namespace TaskService.Domain.Repositories;

public interface IRepositoryWrapper
{
    IIssueRepository Issues { get; }
    IProjectRepository Projects { get; }
    IWorkspaceRepository Workspaces { get; }
    IUnitOfWork UnitOfWork { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
