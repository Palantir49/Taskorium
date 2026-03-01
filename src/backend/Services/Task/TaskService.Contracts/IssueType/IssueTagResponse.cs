namespace TaskService.Contracts.IssueTag;

public record class IssueTagResponse(
    Guid id,
    string name,
    Guid projectId);
