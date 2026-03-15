using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectUpdateHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<ProjectUpdateCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectUpdateCommand request,
        CancellationToken cancellationToken = default)
    {
        var project = await context.Projects.FindAsync([request.id], cancellationToken) ??
                      throw new KeyNotFoundException($"Проект с id: {request.id} не найден");

        project.UpdateName(request.Name);
        project.UpdateDescription(request.Description);

        context.Projects.Update(project);
        await context.SaveChangesAsync(cancellationToken);

        // Инвалидируем кэш:
        var projectCacheKey = $"project_{project.Id}";
        var workspaceProjectsCacheKey = $"projects_by_workspace_{project.WorkspaceId}";


        await cache.RemoveAsync(projectCacheKey, cancellationToken);
        await cache.RemoveAsync(workspaceProjectsCacheKey, cancellationToken);


        return project.ToResponse();
    }
}
