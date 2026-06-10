using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Read.GetProjectByWorkspaceId;

public class GetProjectByWorkspaceIdHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    ICurrentUserContext currentUserContext)
    : IRequestHandler<GetProjectByWorkspaceIdQuery, IEnumerable<ProjectResponse>>
{
    public async Task<IEnumerable<ProjectResponse>> Handle(GetProjectByWorkspaceIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"projects_by_workspace_{request.id}";

        return await cache.GetOrCreateAsync(
            cacheKey,
            async token =>
            {
                var projects = await context.Projects
                    .AsNoTracking()
                    .Include(x => x.ProjectMembers)
                    .Where(x => x.WorkspaceId == request.id)
                    .ToListAsync(token);

                return projects.Select(x => x.ToResponse(currentUserContext.User.Id)).ToList();
            },
            cancellationToken: cancellationToken);
    }
}
