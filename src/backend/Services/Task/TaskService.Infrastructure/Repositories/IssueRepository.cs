using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.IRepositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories;

public class IssueRepository(TaskServiceDbContext context) : IIssueRepository
{
    public async Task AddAsync(Issue task, CancellationToken ct = default)
    {
        await context.AddAsync(task, ct);
    }

    public async Task<Issue?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Issues.FindAsync([id], ct);
    }

    public async Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
    {
        return await context.Issues
            .Where(i => i.ProjectId == projectId)
            .ToListAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await context.SaveChangesAsync(ct);
    }
}
