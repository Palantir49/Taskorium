namespace TaskService.Contracts.Issue.Responses;

public record IssueResponse(
    Guid Id,
    string Name,
    Guid ProjectId,
    //Guid TaskTagId,
    Guid TaskStatusId,
    DateTimeOffset CreatedDate,
    string? Description = null,
    DateTimeOffset? UpdatedDate = null,
    DateTimeOffset? DueDate = null,
    DateTimeOffset? ResolvedDate = null);
