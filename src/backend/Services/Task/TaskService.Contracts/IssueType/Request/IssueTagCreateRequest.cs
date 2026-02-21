namespace TaskService.Contracts.IssueTag.Request;

public record class IssueTagCreateRequest(
    string name,
    Guid projectId,
    string? color);
