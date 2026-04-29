using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Cache.Interfaces;
using TaskService.Application.Features.Users.Get;
using TaskService.Application.Mapping;
using TaskService.Contracts.Common.DTO;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Cache;

public class UserCache(TaskServiceDbContext context, HybridCache cache) : IUserCache
{
    public Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task InvalidateUserByIdCacheAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<GetUserByKeycloakIdResult> GetUserByKeycloakIdAsync(Guid id, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.UserByKeycloakId(id);

        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var existUser = await context.Users
                                .Include(x => x.WorkspaceMembers)
                                .Include(x => x.ProjectMembers)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.KeycloakId == id, ct) ??
                            throw new KeyNotFoundException(
                                $"Пользователь с таким keycloak id {id} не существует");
            var userWorkspaces = existUser.WorkspaceMembers
                .Select(x => new WorkSpaceMemberDto(x.WorkspaceId,
                    x.UserId,
                    x.Role.ToDto()))
                .ToList();

            var userProjects = existUser.ProjectMembers
                .Select(x => new ProjectMemberDto(x.ProjectId,
                    x.UserId,
                    x.Role.ToDto()))
                .ToList();

            return new GetUserByKeycloakIdResult(existUser.Id, existUser.KeycloakId, userProjects, userWorkspaces);
        }, cancellationToken: ct);
    }

    public async Task InvalidateUserByKeycloakIdCacheAsync(Guid id, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.UserByKeycloakId(id);
        await cache.RemoveAsync(cacheKey, ct);
    }
}
