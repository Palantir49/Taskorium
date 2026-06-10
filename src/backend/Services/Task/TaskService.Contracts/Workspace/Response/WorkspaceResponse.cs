using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Workspace.Response;

public record WorkspaceResponse(
    Guid Id,
    string Name,
    DateTimeOffset CreatedDate,
    WorkspaceRolesDto Role);
