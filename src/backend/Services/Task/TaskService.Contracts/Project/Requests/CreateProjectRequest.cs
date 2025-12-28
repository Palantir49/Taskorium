namespace TaskService.Contracts.Project.Requests;

public record CreateProjectRequest(
    string Name,
    string Description,
    Guid WorkspaceId);
