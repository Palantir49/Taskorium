using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Read.GetProjectById;

public class ProjectGetByIdHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    ICurrentUserContext currentUserContext)
    : IRequestHandler<GetProjectByIdQuery, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(GetProjectByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"project_{request.Id}";

        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var project =
                await context.Projects.Include(element => element.ProjectMembers)
                    .FirstAsync(element => element.Id == request.Id, cancellationToken) ??
                throw new KeyNotFoundException($"Проект с id: {request.Id} не найден");

            return project.ToResponse(currentUserContext.User.Id);
        }, cancellationToken: cancellationToken);
    }
}
