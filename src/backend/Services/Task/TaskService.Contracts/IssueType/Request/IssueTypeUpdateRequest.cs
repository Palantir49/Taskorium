namespace TaskService.Contracts.IssueType.Request;

public record class IssueTypeUpdateRequest(
    Guid id,
    string name,
    string? color);
