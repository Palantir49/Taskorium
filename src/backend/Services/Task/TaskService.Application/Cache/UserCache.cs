using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using TaskService.Application.Cache.Contracts.Keycloak;
using TaskService.Application.Cache.Contracts.User;
using TaskService.Application.Cache.Interfaces;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Cache;

public class UserCache(TaskServiceDbContext context, HybridCache cache, ILogger<UserCache> log) : IUserCache
{
    public async Task<UserMetaCache?> GetMetaAsync(Guid userId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.UserMeta(userId);
        var cacheTag = CacheKeys.UserTag(userId);

        return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
        {
            log.LogInformation("Cache miss for user meta {cacheKey}", cacheKey);

            User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, ct);

            //TODO: а как правильно быть в данной ситуации? кэшировать Null как-то такое себе. С другой стороны неверные guid будет всегда лететь в БД
            if (user == null)
            {
                log.LogWarning("An attempt to get a non-existent workspace an ID {id}", userId);
                return null;
            }

            return new UserMetaCache(
            Id: user.Id,
            UserName: user.Username.ToString(),
            Email: user.Email.ToString(),
            FullName: user.FullName);
        },

        options: new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(2),
            LocalCacheExpiration = TimeSpan.FromMinutes(1),
        },

        tags: [cacheTag],
        cancellationToken: ct
        );
    }

    public async Task<IReadOnlyList<UserProjectCache>> GetProjectsAsync(Guid userId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.UserMeta(userId);
        var cacheTag = CacheKeys.UserTag(userId);

        return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
        {
            log.LogInformation("Cache miss for user meta {cacheKey}", cacheKey);

            return await context.ProjectMembers
            .Include(x => x.Project)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new UserProjectCache(
                Id: x.Project.Id,
                Name: x.Project.Name.ToString(),
                Description: x.Project.Description,
                Abbreviation: x.Project.Abbreviation,
                Role: x.Role))
            .ToListAsync(ct);
        },

        options: new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(2),
            LocalCacheExpiration = TimeSpan.FromMinutes(1),
        },

        tags: [cacheTag],
        cancellationToken: ct
        );
    }

    public async Task<IReadOnlyList<UserWorkspaceCache>> GetWorkspacesAsync(Guid userId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.UserWorkspaces(userId);
        var cacheTag = CacheKeys.UserTag(userId);

        return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
        {
            log.LogInformation("Cache miss for user workspaces {cacheKey}", cacheKey);

            return await context.WorkspaceMembers
            .Include(x => x.Workspace)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new UserWorkspaceCache(
                Id: x.Workspace.Id,
                Name: x.Workspace.Name.ToString(),
                CreateDate: x.Workspace.CreatedDate,
                Role: x.Role))
            .ToListAsync(ct);
        },

        options: new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(2),
            LocalCacheExpiration = TimeSpan.FromMinutes(1),
        },

        tags: [cacheTag],
        cancellationToken: ct
        );
    }
    //TODO: разбить на части meta, ws, proj, если будет время
    public async Task<KeyCloakCache> GetUserIdByKeycloakIdAsync(Guid keycloakId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.KeycloakMeta(keycloakId);
        var cacheTag = CacheKeys.KeycloakTag(keycloakId);

        return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
        {
            var existUser = await context.Users
                                .Include(u => u.WorkspaceMembers)
                                    .ThenInclude(wm => wm.Workspace)
                                .Include(u => u.ProjectMembers)
                                .ThenInclude(pm => pm.Project)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync(x => x.KeycloakId == keycloakId, ct) ??
                            throw new KeyNotFoundException(
                                $"Пользователь с таким keycloak id {keycloakId} не существует");
            var userWorkspaces = existUser.WorkspaceMembers
                .Select(x => new WorkspaceMemberCache(x.WorkspaceId,
                    x.UserId,
                    x.Role))
                .ToList();

            var userProjects = existUser.ProjectMembers
                .Select(x => new ProjectMemberCache(x.ProjectId,
                    x.UserId,
                    x.Role))
                .ToList();

            return new KeyCloakCache(existUser.Id, userWorkspaces, userProjects);
        },
        options: new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(2),
            LocalCacheExpiration = TimeSpan.FromMinutes(1),
        },

        tags: [cacheTag],
        cancellationToken: ct);
    }


    public async Task InvalidateAllAsync(Guid userId, Guid keycloakId, CancellationToken ct = default)
    {
        var cacheTag = CacheKeys.UserTag(userId);
        await cache.RemoveByTagAsync(cacheTag, ct);

        var keycloakTag = CacheKeys.KeycloakTag(keycloakId);
        await cache.RemoveByTagAsync(keycloakTag, ct);
    }

    public async Task InvalidateMetaAsync(Guid userId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.UserMeta(userId);
        await cache.RemoveAsync(cacheKey, ct);
    }

    public async Task InvalidateProjectsAsync(Guid userId, Guid keycloakId, CancellationToken ct = default)
    {
        var cacheTag = CacheKeys.UserProjTag(userId);
        await cache.RemoveByTagAsync(cacheTag, ct);

        var keycloakTag = CacheKeys.KeycloakTag(keycloakId);
        await cache.RemoveByTagAsync(keycloakTag, ct);
    }

    public async Task InvalidateWorkspacesAsync(Guid userId, Guid keycloakId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.UserWorkspaces(userId);
        await cache.RemoveAsync(cacheKey, ct);

        var keycloakTag = CacheKeys.KeycloakTag(keycloakId);
        await cache.RemoveByTagAsync(keycloakTag, ct);
    }
}
