using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Cache.Contracts.User;

public record UserProjectCache(
    Guid Id,
    string Name,
    string? Description,
    string Abbreviation,
    ProjectRoles Role);
