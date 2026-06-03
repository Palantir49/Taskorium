using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Project.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Read.GetProjectMembers;

public class GetProjectMembersHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<GetProjectMembersQuery, ProjectMembersResponse>
{
    public async Task<ProjectMembersResponse> Handle(GetProjectMembersQuery request,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"projectMembers_{request.Id}";

        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var existProject = await context.Projects.Include(x => x.ProjectMembers)
                .ThenInclude(x => x.User)
                .Include(x => x.Issues)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (existProject is null)
            {
                throw new ArgumentNullException($"Проект с id: {request.Id} не найден");
            }

            var members = new List<ProjectUserDto>();

            foreach (var x in existProject.ProjectMembers)
            {
                members.Add(new ProjectUserDto(x.User.Id, x.User.KeycloakId, x.Role.ToDto(),
                    x.JoinedAt, x.User.Email.Value, x.User.Username.Value));
            }

            return new ProjectMembersResponse(existProject.Id, existProject.Name.Value,
                members);
        }, cancellationToken: cancellationToken);
    }
}
