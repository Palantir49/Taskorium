using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectGetWorkspaceIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<ProjectGetByWorkspaceIdQuery, IEnumerable<ProjectResponse>>
{
    public async Task<IEnumerable<ProjectResponse>> Handle(ProjectGetByWorkspaceIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"projects_by_workspace_{request.id}";

        return await cache.GetOrCreateAsync(
            cacheKey,
            async token =>
            {
                var projects = await context.Projects
                    .AsNoTracking()
                    .Where(x => x.WorkspaceId == request.id)
                    .ToListAsync(token);

                return projects.Select(x => x.ToResponse()).ToList();
            },
            cancellationToken: cancellationToken);
    }
}
