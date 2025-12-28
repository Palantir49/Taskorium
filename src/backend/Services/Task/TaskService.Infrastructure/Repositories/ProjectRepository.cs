using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories;

public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
{
    public ProjectRepository(TaskServiceDbContext context) : base(context) { }

    public async Task<List<Project>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken ct = default)
    {
        return await _context.Projects.Where(x => x.WorkspaceId == workspaceId).ToListAsync(ct);
    }
}
