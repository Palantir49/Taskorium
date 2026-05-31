using TaskService.Application.Cache.Contracts.Keycloak;
using TaskService.Application.Cache.Contracts.User;

namespace TaskService.Application.Cache.Interfaces;

public interface IUserCache
{
    Task<KeyCloakCache> GetUserIdByKeycloakIdAsync(Guid keycloakId, CancellationToken ct = default);
    Task<UserMetaCache?> GetMetaAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<UserWorkspaceCache>> GetWorkspacesAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<UserProjectCache>> GetProjectsAsync(Guid iduserId, CancellationToken ct = default);

    Task InvalidateAllAsync(Guid userId, Guid keycloakId, CancellationToken ct = default);
    Task InvalidateMetaAsync(Guid userId, CancellationToken ct = default);
    Task InvalidateWorkspacesAsync(Guid userId, Guid keycloakId, CancellationToken ct = default);
    Task InvalidateProjectsAsync(Guid userId, Guid keycloakId, CancellationToken ct = default);
}
