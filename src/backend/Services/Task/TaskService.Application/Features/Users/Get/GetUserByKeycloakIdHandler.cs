using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Users.Get;

public class GetUserByKeycloakIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<GetUserByKeycloakIdQuery, GetUserByKeycloakIdResult>
{
    public async Task<GetUserByKeycloakIdResult> Handle(GetUserByKeycloakIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"user_by_keycloak_id_{query.KeycloakId}";

        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var existUser = await context.Users
                                .Include(x => x.WorkspaceMembers)
                                .Include(x => x.ProjectMembers)
                                .FirstOrDefaultAsync(x => x.KeycloakId == query.KeycloakId, cancellationToken) ??
                            throw new ArgumentNullException("Пользователь с таким keycloak id не существует",
                                nameof(query.KeycloakId));
            var userWorkspaces = existUser.WorkspaceMembers
                .Select(x => new WorkSpaceMemberDto(x.WorkspaceId,
                    x.UserId,
                    new RoleDto(x.Role.ToString())))
                .ToList();

            var userProjects = existUser.ProjectMembers
                .Select(x => new ProjectMemberDto(x.ProjectId,
                    x.UserId,
                    new RoleDto(x.Role.ToString())))
                .ToList();

            return new GetUserByKeycloakIdResult(existUser.Id, existUser.KeycloakId, userProjects, userWorkspaces);
        }, cancellationToken: cancellationToken);
    }
}
