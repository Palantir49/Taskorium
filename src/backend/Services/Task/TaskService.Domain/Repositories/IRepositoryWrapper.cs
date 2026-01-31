using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories;

public interface IRepositoryWrapper
{
    IIssueRepository Issues { get; }
    IIssueStatusRepository IssueStatus { get; }
    IIssueTypeRepository IssueType { get; }
    IProjectRepository Projects { get; }
    IWorkspaceRepository Workspaces { get; }
    IUserRepository Users { get; }
    IUnitOfWork UnitOfWork { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
