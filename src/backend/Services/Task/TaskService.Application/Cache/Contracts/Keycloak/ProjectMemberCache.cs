using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Cache.Contracts.Keycloak;

public record ProjectMemberCache(Guid ProjectId, Guid UserId, ProjectRoles Role);
