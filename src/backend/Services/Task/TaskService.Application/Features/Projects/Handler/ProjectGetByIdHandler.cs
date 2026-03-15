using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

internal class ProjectGetByIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<ProjectGetByIdQuery, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"project_{request.Id}";

        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var project = await context.Projects.FindAsync([request.Id], cancellationToken) ??
                          throw new KeyNotFoundException($"Проект с id: {request.Id} не найден");

            return project.ToResponse();
        }, cancellationToken: cancellationToken);
    }
}
