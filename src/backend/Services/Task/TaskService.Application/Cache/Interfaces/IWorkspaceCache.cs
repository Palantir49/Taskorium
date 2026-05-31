using TaskService.Application.Cache.Contracts.User;
using TaskService.Application.Cache.Contracts.Workspace;

namespace TaskService.Application.Cache.Interfaces;

public interface IWorkspaceCache
{
    Task<WorkspaceMetaCache?> GetMetaAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<WorkspaceUserCache>> GetUsersAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<UserProjectCache>> GetProjectsAsync(Guid id, Guid userId, CancellationToken ct = default);

    Task InvalidateAllAsync(Guid id, CancellationToken ct = default);
    Task InvalidateMetaAsync(Guid id, CancellationToken ct = default);
    Task InvalidateProjectsAsync(Guid id, CancellationToken ct = default);
    Task InvalidateUsersAsync(Guid id, CancellationToken ct = default);
}
