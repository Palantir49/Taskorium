using TaskService.Application.Features.Users.Get;
using TaskService.Domain.Entities;

namespace TaskService.Application.Cache.Interfaces;

public interface IUserCache
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
    Task<GetUserByKeycloakIdResult> GetUserByKeycloakIdAsync(Guid id, CancellationToken ct = default);

    Task InvalidateUserByIdCacheAsync(CancellationToken ct = default);
    Task InvalidateUserByKeycloakIdCacheAsync(Guid id, CancellationToken ct = default);
}
