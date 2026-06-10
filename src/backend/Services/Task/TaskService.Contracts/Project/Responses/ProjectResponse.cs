using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Project.Responses;

public record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    string Abbreviation,
    Guid WorkspaceId,
    DateTimeOffset CreatedDate,
    ProjectRolesDto Role);
