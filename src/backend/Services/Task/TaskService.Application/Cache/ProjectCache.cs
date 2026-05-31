using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using TaskService.Application.Cache.Contracts.Project;
using TaskService.Application.Cache.Contracts.Workspace;
using TaskService.Application.Cache.Interfaces;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Cache
{
    public class ProjectCache(TaskServiceDbContext context, HybridCache cache, ILogger<WorkspaceCache> log) : IProjectCache
    {
        public async Task<ProjectMetaCache?> GetMetaAsync(Guid id, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.ProjectMeta(id);
            var cacheTag = CacheKeys.ProjectTag(id);

            return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
            {
                //TODO: лог информации или дебага?
                log.LogInformation("Cache miss for project meta {cacheKey}", cacheKey);

                Project? project = await context.Projects
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

                //TODO: Прочел, что можно добавить проверку на наличие и кэшить как 404, чтобы избегать ситуаций, когда workspace нет
                if (project == null)
                {
                    log.LogWarning("An attempt to get a non-existent project an ID {id}", id);
                    return null;
                }

                return new ProjectMetaCache(
                Id: project.Id,
                Name: project.Name.ToString(),
                Description: project.Description,
                Abbreviation: project.Abbreviation,
                StartDate: project.CreatedDate,
                FinishDate: project.FinishDate
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

        public async Task<IReadOnlyList<StatusCache>> GetStatusesAsync(Guid id, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.ProjectStatuses(id);
            var cacheTag = CacheKeys.ProjectTag(id);

            return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
            {
                log.LogInformation("Cache miss for project statuses {cacheKey}", cacheKey);

                return await context.IssueStatus
                .AsNoTracking()
                .Where(m => m.ProjectId == id)
                .Select(m => new StatusCache(
                    Id: m.Id,
                    Name: m.Name.ToString(),
                    Position: m.Position,
                    Color: "#000000"
                    ))
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

        public async Task<IReadOnlyList<TagCache>> GetTagsAsync(Guid id, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.ProjectIssueTags(id);
            var cacheTag = CacheKeys.ProjectTag(id);

            return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
            {
                log.LogInformation("Cache miss for project tags {cacheKey}", cacheKey);

                return await context.Tags
                .AsNoTracking()
                .Where(m => m.ProjectId == id)
                .Select(m => new TagCache(
                    Id: m.Id,
                    Name: m.Name.ToString()
                    ))
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

        public async Task<IReadOnlyList<ProjectUserCache>> GetUsersAsync(Guid id, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.ProjectUsers(id);
            var cacheTag = CacheKeys.ProjectTag(id);

            return await cache.GetOrCreateAsync(cacheKey, async (ct) =>
            {
                log.LogInformation("Cache miss for project users {cacheKey}", cacheKey);

                return await context.ProjectMembers
                .Include(x => x.User)
                .AsNoTracking()
                .Where(m => m.ProjectId == id)
                .Select(m => new ProjectUserCache(
                    userId: m.User.Id,
                    Username: m.User.Username.ToString(),
                    Role: m.Role
                    ))
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

        public async Task InvalidateAllAsync(Guid id, CancellationToken ct = default)
        {
            var cacheTag = CacheKeys.ProjectTag(id);
            await cache.RemoveByTagAsync(cacheTag, ct);
        }

        public async Task InvalidateMetaAsync(Guid id, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.ProjectMeta(id);
            await cache.RemoveAsync(cacheKey, ct);
        }

        public async Task InvalidateStatusesAsync(Guid id, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.ProjectStatuses(id);
            await cache.RemoveAsync(cacheKey, ct);
        }

        public async Task InvalidateTagsAsync(Guid id, CancellationToken ct = default)
        {
            var cacheKey = CacheKeys.ProjectIssueTags(id);
            await cache.RemoveAsync(cacheKey, ct);
        }
    }
}
