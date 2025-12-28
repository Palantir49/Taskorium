using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.IRepositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly TaskServiceDbContext _context;

        public WorkspaceRepository(TaskServiceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Workspace task, CancellationToken ct = default)
        {
            await _context.AddAsync(task);
        }

        public async Task<Workspace?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Workspaces.FindAsync(new object[] { id }, ct);
        }

        public async Task<List<Workspace>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default)
        {
            return await _context.Workspaces
                .Where(i => i.OwnerId == ownerId)
                .ToListAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
