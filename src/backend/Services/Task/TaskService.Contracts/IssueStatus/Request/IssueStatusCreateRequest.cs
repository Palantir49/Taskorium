namespace TaskService.Contracts.IssueStatus.Request;

public record class IssueStatusCreateRequest(
    string name,
    Guid projectId,
    string type,
    int position,
    string? color);

