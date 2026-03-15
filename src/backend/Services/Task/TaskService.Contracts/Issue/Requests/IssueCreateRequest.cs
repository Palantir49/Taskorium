namespace TaskService.Contracts.Issue.Requests;

public record class IssueCreateRequest(
    string Name,
    Guid ProjectId,
    int NumberIssueType,
    int NumberIssuePriority,
    string? Description = null,
    DateTimeOffset? DueDate = null);
