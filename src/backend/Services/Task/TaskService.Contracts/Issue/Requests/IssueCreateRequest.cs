namespace TaskService.Contracts.Issue.Requests;

public record class IssueCreateRequest(
    string Name,
    Guid ProjectId,
    Guid IssueTagId,
    Guid IssueStatusId,
    string? Description = null,
    DateTimeOffset? DueDate = null);
