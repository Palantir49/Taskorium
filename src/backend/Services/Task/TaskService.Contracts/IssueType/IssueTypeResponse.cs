namespace TaskService.Contracts.IssueType;

public record class IssueTypeResponse(
    Guid id,
    string name,
    Guid projectId,
    string? color);
