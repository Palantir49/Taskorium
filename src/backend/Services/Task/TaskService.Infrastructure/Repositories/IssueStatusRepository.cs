using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories
{
    internal class IssueStatusRepository : RepositoryBase<IssueStatus>, IIssueStatusRepository
    {
        public IssueStatusRepository(TaskServiceDbContext context) : base(context) { }

        public async Task<List<IssueStatus>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        {
            return await _context.IssueStatus
                .Where(x => x.ProjectId == projectId)
                .ToListAsync(ct);
        }
    }
}
