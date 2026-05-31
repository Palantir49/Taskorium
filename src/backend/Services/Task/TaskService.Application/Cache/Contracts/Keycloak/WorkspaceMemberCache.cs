using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Cache.Contracts.Keycloak;

public record WorkspaceMemberCache(Guid WorkspaceId, Guid UserId, WorkspaceRoles Role);
