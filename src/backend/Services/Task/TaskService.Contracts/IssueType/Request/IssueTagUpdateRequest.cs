namespace TaskService.Contracts.IssueTag.Request;

public record class IssueTagUpdateRequest(
    Guid id,
    string name,
    string? color);
