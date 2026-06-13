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
        var cacheKey = $"projects_by_workspace_{request.Id}_{currentUserContext.User.Id}";

        return await cache.GetOrCreateAsync(
            cacheKey,
            async token => await GetProjectByWorkspaceIdFromDb(request.Id, token),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ProjectResponse>> GetProjectByWorkspaceIdFromDb(Guid id, CancellationToken cancellationToken)
    {
        var projects = await context.Projects
                    .AsNoTracking()
                    .Include(x => x.ProjectMembers)
                    .Where(x => x.WorkspaceId == id)
                    .ToListAsync(cancellationToken);

        //projects = projects.Where(
        //    x => x.ProjectMembers.Any(x => x.UserId == currentUserContext.User.Id))
        //    .ToList();

        return projects.Select(x => x.ToResponse(currentUserContext.User.Id)).ToList();
    }
}
