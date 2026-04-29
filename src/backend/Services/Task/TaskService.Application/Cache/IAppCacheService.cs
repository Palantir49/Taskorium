using TaskService.Application.Features.Users.Get;
using TaskService.Domain.Entities;

namespace TaskService.Application.Cache;

public interface IAppCacheService
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
    Task<GetUserByKeycloakIdResult> GetUserByKeycloakIdAsync(Guid id, CancellationToken ct = default);

    Task InvalidateUserByIdCacheAsync(CancellationToken ct = default);
    Task InvalidateUserByKeycloakIdCacheAsync(Guid id, CancellationToken ct = default);
}
