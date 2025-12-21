using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.IRepositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories
{
    internal class ProjectRepository : IProjectRepository
    {
        private readonly TaskServiceDbContext _context;

        public ProjectRepository(TaskServiceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project project, CancellationToken ct = default)
        {
            await _context.AddAsync(project, ct);
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Projects.FindAsync(new object[] { id }, ct);
        }

        public async Task<List<Project>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken ct = default)
        {
            return await _context.Projects.Where(x => x.WorkspaceId == workspaceId).ToListAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
