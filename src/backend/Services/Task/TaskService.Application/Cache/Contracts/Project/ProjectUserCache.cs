using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Cache.Contracts.Project;

public record ProjectUserCache(Guid userId, string Username, ProjectRoles Role);
