using TaskService.Contracts.Enum;

namespace TaskService.Application.Features.Workspaces.Write.CreateWorkspace;

public record CreateWorkspaceResult(Guid Id, string Name, WorkspaceRolesDto Role);
