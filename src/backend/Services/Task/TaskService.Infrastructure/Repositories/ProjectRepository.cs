using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories;

internal class ProjectRepository(TaskServiceDbContext context) : IProjectRepository
{
    public async Task AddAsync(Project project, CancellationToken ct = default)
    {
        await context.AddAsync(project, ct);
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Projects.FindAsync([id], ct);
    }

    public async Task<List<Project>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken ct = default)
    {
        return await context.Projects.Where(x => x.WorkspaceId == workspaceId).ToListAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await context.SaveChangesAsync(ct);
    }
}
