namespace TaskService.Contracts.Issue.Requests;

public record class IssueCreateRequest(
    string Name,
    Guid ProjectId,
    //Guid IssueTagId,
    //Guid IssueStatusId,
    int numberIssueType,
    string? Description = null,
    DateTimeOffset? DueDate = null);
