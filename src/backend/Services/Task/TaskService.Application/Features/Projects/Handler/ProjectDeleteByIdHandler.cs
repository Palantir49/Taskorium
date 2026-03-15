using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectDeleteByIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<ProjectDeleteByIdCommand, int>
{
    public async Task<int> Handle(ProjectDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        var project = await context.Projects.FindAsync([request.id], cancellationToken) ??
                      throw new KeyNotFoundException($"Проект с id: {request.id} не найден");

        var issues = await context.Issues
            .Where(x => x.ProjectId == project.Id)
            .ToListAsync(cancellationToken);

        if (issues.Count > 0)
        {
            throw new InvalidOperationException("Нельзя удалить проект, пока существуют связанные задачи");
        }

        var statuses = await context.IssueStatus
            .Where(x => x.ProjectId == project.Id)
            .ToListAsync(cancellationToken);

        var types = await context.IssueType
            .Where(x => x.ProjectId == project.Id)
            .ToListAsync(cancellationToken);

        context.IssueStatus.RemoveRange(statuses);
        context.IssueType.RemoveRange(types);
        context.Projects.Remove(project);

        var deletedCount = await context.SaveChangesAsync(cancellationToken);

        // Инвалидируем кэш:
        var projectCacheKey = $"project_{project.Id}";
        var workspaceProjectsCacheKey = $"projects_by_workspace_{project.WorkspaceId}";


        await cache.RemoveAsync(projectCacheKey, cancellationToken);
        await cache.RemoveAsync(workspaceProjectsCacheKey, cancellationToken);


        return deletedCount;
    }
}
