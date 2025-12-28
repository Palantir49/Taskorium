using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories;

public class IssueRepository : RepositoryBase<Issue>, IIssueRepository
{
    public IssueRepository(TaskServiceDbContext context) : base(context) { }

    public async Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
    {
        return await _context.Issues
            .Where(i => i.ProjectId == projectId)
            .ToListAsync(ct);
    }
}
