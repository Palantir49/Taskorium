using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Cache.Contracts.Workspace;

public record WorkspaceUserCache(Guid UserId, string Username, WorkspaceRoles Role);
