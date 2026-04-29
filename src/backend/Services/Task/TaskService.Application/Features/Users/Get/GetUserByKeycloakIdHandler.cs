using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Cache;
using TaskService.Application.Features.WorkspaceMembers;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Users.Get;

public class GetUserByKeycloakIdHandler(/*TaskServiceDbContext context,*/ IAppCacheService cache)
    : IRequestHandler<GetUserByKeycloakIdQuery, GetUserByKeycloakIdResult>
{
    public async Task<GetUserByKeycloakIdResult> Handle(GetUserByKeycloakIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await cache.GetUserByKeycloakIdAsync(query.KeycloakId ??
            throw new ArgumentException("keycloak id не может быть пустым"));
    }
}
