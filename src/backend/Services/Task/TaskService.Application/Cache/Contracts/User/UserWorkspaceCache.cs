using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Cache.Contracts.User;

public record UserWorkspaceCache(Guid Id, string Name, DateTimeOffset CreateDate, WorkspaceRoles Role);
