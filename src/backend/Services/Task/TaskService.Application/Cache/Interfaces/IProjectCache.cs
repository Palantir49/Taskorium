using TaskService.Application.Cache.Contracts.Keycloak;
using TaskService.Application.Cache.Contracts.Project;

namespace TaskService.Application.Cache.Interfaces;

public interface IProjectCache
{
    Task<ProjectMetaCache?> GetMetaAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<ProjectUserCache>> GetUsersAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<TagCache>> GetTagsAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<StatusCache>> GetStatusesAsync(Guid id, CancellationToken ct = default);

    Task InvalidateAllAsync(Guid id, CancellationToken ct = default);
    Task InvalidateMetaAsync(Guid id, CancellationToken ct = default);
    Task InvalidateTagsAsync(Guid id, CancellationToken ct = default);
    Task InvalidateStatusesAsync(Guid id, CancellationToken ct = default);
}

