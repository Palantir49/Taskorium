using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities;

namespace TaskService.Domain.IRepositories
{
    public interface IWorkspaceRepository
    {
        Task AddAsync(Workspace workspace, CancellationToken ct = default);
        Task<Workspace?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Workspace>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
