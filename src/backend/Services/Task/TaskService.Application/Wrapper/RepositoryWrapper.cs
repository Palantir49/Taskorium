using TaskService.Domain.Repositories;

namespace TaskService.Application.Wrapper
{
    internal class RepositoryWrapper : IRepositoryWrapper
    {

        private readonly IIssueRepository _issue;
        private readonly IProjectRepository _project;
        private readonly IWorkspaceRepository _workspace;
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryWrapper(
            IIssueRepository issue,
            IProjectRepository project,
            IWorkspaceRepository workspace,
            IUnitOfWork unitOfWork)
        {
            _issue = issue;
            _project = project;
            _workspace = workspace;
            _unitOfWork = unitOfWork;
        }

        public IIssueRepository Issues => _issue;

        public IProjectRepository Projects => _project;

        public IWorkspaceRepository Workspaces => _workspace;

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public void SaveChangesAsync(CancellationToken ct = default)
        {
            _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
