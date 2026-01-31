using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories;

internal class IssueTypeRepository : RepositoryBase<IssueType>, IIssueTypeRepository
{
    internal IssueTypeRepository(TaskServiceDbContext context) : base(context) { }

    public async Task<List<IssueType>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
    {
        return await _context.IssueType.Where(x => x.ProjectId == projectId).ToListAsync();
    }
}
