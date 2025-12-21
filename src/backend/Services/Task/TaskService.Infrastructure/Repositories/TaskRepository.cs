using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.IRepositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskServiceDbContext _context;

        public TaskRepository(TaskServiceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Issue task, CancellationToken ct = default)
        {
            await _context.AddAsync(task);
        }

        public async Task<Issue?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Issues.FindAsync(new object[] { id }, ct);
        }

        public async Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        {
            return await _context.Issues
                .Where(i => i.ProjectId == projectId)
                .ToListAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
