using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.IRepositories;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Wrapper
{
    internal interface IRepositoryWrapper
    {
        IIssueRepository Issues { get; }
        IProjectRepository Projects { get; }
        IWorkspaceRepository Workspaces { get; }

        void SaveChangesAsync(CancellationToken ct = default);
    }
}
