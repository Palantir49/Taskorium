namespace TaskService.Contracts.IssueStatus.Request;

public record class IssueStatusUpdateRequest(
    string name,
    string type,
    int position,
    string? color);

