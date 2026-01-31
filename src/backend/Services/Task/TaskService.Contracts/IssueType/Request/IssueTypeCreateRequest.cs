namespace TaskService.Contracts.IssueType.Request;

public record class IssueTypeCreateRequest(
    string name,
    Guid projectId,
    string? color);
