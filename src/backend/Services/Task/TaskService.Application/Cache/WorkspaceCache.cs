using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using TaskService.Application.Cache.Contracts.Project;
using TaskService.Application.Cache.Contracts.User;
using TaskService.Application.Cache.Contracts.Workspace;
using TaskService.Application.Cache.Interfaces;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Cache;

public class WorkspaceCache(TaskServiceDbContext context, HybridCache cache, ILogger<WorkspaceCache> log) : IWorkspaceCache
{
    /// <summary>
    /// Получение кэша свойств рабочей области
    /// </summary>
    /// <param name="wsId">идентификатор workspace</param>
    /// <param name="ct"></param>
    /// <returns>WorkspaceMetaCache если объект был получен или null - если не найден</returns>
    public async Task<WorkspaceMetaCache?> GetMetaAsync(Guid wsId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.WorkspaceMeta(wsId);
        var cacheTag = CacheKeys.WorkspaceTag(wsId);

        return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
        {
            //TODO: лог информации или дебага?
            log.LogInformation("Cache miss for workspace meta {cacheKey}", cacheKey);

            Workspace? workspace = await context.Workspaces
            .Include(x => x.WorkspaceMembers)
            .AsNoTracking().FirstOrDefaultAsync(x => x.Id == wsId, ct);

            //TODO: Прочел, что можно добавить проверку на наличие и кэшить как 404, чтобы избегать ситуаций, когда workspace нет
            if (workspace == null)
            {
                log.LogWarning("An attempt to get a non-existent workspace an ID {id}", wsId);
                return null;
            }

            return new WorkspaceMetaCache(
            Id: workspace.Id,
            Name: workspace.Name.ToString(),
            CreateDate: workspace.CreatedDate
            );
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

    /// <summary>
    /// Получение кэша юзеров, связанных с рабочей областью
    /// </summary>
    /// <param name="wsId">идентификатор workspace</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<WorkspaceUserCache>> GetUsersAsync(Guid wsId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.WorkspaceUsers(wsId);
        var cacheTag = CacheKeys.WorkspaceTag(wsId);

        return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
        {
            log.LogInformation("Cache miss for workspace users {cacheKey}", cacheKey);
            return await context.WorkspaceMembers
            .Include(x => x.User)
            .AsNoTracking()
            .Where(m => m.WorkspaceId == wsId)
            .Select(m => new WorkspaceUserCache(
                UserId: m.UserId,
                Username: m.User.Username.ToString(),
                Role: m.Role))
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

    /// <summary>
    /// Получение кэша проектов, связанных с рабочей областью
    /// </summary>
    /// <param name="wsId">идентификатор workspace</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<UserProjectCache>> GetProjectsAsync(Guid wsId, Guid userId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.WorkspaceProjects(wsId, userId);

        var cacheWsTag = CacheKeys.WorkspaceTag(wsId);
        var cacheWsProjTag = CacheKeys.WorkspaceProjTag(wsId);
        var cacheUserTag = CacheKeys.UserTag(userId);
        var cacheUserProjTag = CacheKeys.UserProjTag(userId);

        return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
        {
            log.LogInformation("Cache miss for projects {cacheKey}", cacheKey);

            return await context.Projects
            .Include(x => x.ProjectMembers)
            .AsNoTracking()
            .Where(x => x.WorkspaceId == wsId)
            .Select(x =>
            new UserProjectCache(
                Id: x.Id,
                Name: x.Name.ToString(),
                Description: x.Description,
                Abbreviation: x.Abbreviation,
                Role: x.ProjectMembers.First(x => x.UserId == userId).Role))
            .ToListAsync(ct);
        },

        options: new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(2),
            LocalCacheExpiration = TimeSpan.FromMinutes(1),
        },

        tags: [cacheWsTag, cacheWsProjTag, cacheUserTag, cacheUserProjTag],
        cancellationToken: ct
        );
    }

    /// <summary>
    /// Инвалидация всего кэша связанного с рабочей областью
    /// </summary>
    /// <param name="wsId">идентификатор workspace</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task InvalidateAllAsync(Guid wsId, CancellationToken ct = default)
    {
        var cacheTag = CacheKeys.WorkspaceTag(wsId);
        await cache.RemoveByTagAsync(cacheTag, ct);
    }

    /// <summary>
    /// Инвалидация кэша свойств рабочей области
    /// </summary>
    /// <param name="wsId">идентификатор workspace</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task InvalidateMetaAsync(Guid wsId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.WorkspaceMeta(wsId);
        await cache.RemoveAsync(cacheKey, ct);
    }

    /// <summary>
    /// Инвалидация кэша проектов, связанных с рабочей областью
    /// </summary>
    /// <param name="wsId">идентификатор workspace</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task InvalidateProjectsAsync(Guid wsId, CancellationToken ct = default)
    {
        var cacheTag = CacheKeys.WorkspaceProjTag(wsId);
        await cache.RemoveByTagAsync(cacheTag, ct);
    }

    /// <summary>
    /// Инвалидация кэша юзеров, связанных с рабочей областью
    /// </summary>
    /// <param name="wsId">идентификатор workspace</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task InvalidateUsersAsync(Guid wsId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeys.WorkspaceUsers(wsId);
        await cache.RemoveAsync(cacheKey, ct);
    }
}
