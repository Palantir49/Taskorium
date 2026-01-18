using TaskService.Domain.Repositories;

namespace TaskService.Infrastructure.Repositories;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly IIssueRepository _issue;
    private readonly IProjectRepository _project;
    private readonly IWorkspaceRepository _workspace;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;
    public RepositoryWrapper(
        IIssueRepository issue,
        IProjectRepository project,
        IWorkspaceRepository workspace,
        IUserRepository users,
        IUnitOfWork unitOfWork)
    {
        _issue = issue;
        _project = project;
        _workspace = workspace;
        _unitOfWork = unitOfWork;
        _users = users;
    }
    public IUserRepository Users => _users;
    public IIssueRepository Issues => _issue;

    public IProjectRepository Projects => _project;

    public IWorkspaceRepository Workspaces => _workspace;

    public IUnitOfWork UnitOfWork => _unitOfWork;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _unitOfWork.SaveChangesAsync(ct);
    }
}
