using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Commands.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectCreateHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<ProjectCreateCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectCreateCommand request,
        CancellationToken cancellationToken = default)
    {
        _ = await context.Workspaces.FindAsync([request.WorkspaceId], cancellationToken) ??
            throw new KeyNotFoundException($"Рабочая область с id: {request.WorkspaceId} не найдена");

        var project = Project.Create(
            request.Name,
            request.Description,
            request.WorkspaceId
        );
        //TODO: добавить создание статусов и типа
        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var cacheKey = $"projects_by_workspace_{project.WorkspaceId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return project.ToResponse();
    }
}
