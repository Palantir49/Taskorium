using System.ComponentModel.DataAnnotations;
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

        return await cache.GetOrCreateAsync(cacheKey,
            async ct => await GetProjectByIdFromDbAsync(request.Id, ct),
            cancellationToken: cancellationToken);
    }

    public async Task<ProjectResponse> GetProjectByIdFromDbAsync(Guid id, CancellationToken cancellationToken)
    {
        var project =
                await context.Projects.Include(element => element.ProjectMembers)
                    .FirstOrDefaultAsync(element => element.Id == id, cancellationToken) ??
                throw new KeyNotFoundException($"Проект с id: {id} не найден");

        if (project.ProjectMembers.Count == 0)
            throw new ValidationException("Проект не содержит пользователей");

        if(!project.ProjectMembers.Any(x => x.UserId == currentUserContext.User.Id))
            throw new ValidationException("Пользователь не имеет доступа к проекту");

        return project.ToResponse(currentUserContext.User.Id);
    }
}
