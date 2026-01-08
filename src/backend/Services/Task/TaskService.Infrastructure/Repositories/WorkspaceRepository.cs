using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories
{
    internal class WorkspaceRepository : RepositoryBase<Workspace>, IWorkspaceRepository
    {
        public WorkspaceRepository(TaskServiceDbContext context) : base(context) { }

        public async Task<List<Workspace>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default)
        {
            return await _context.Workspaces
                .Where(i => i.OwnerId == ownerId)
                .ToListAsync(ct);
        }
    }
}
