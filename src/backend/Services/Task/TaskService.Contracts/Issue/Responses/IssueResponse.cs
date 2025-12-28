namespace TaskService.Contracts.Issue.Responses;

public record IssueResponse(
    Guid Id,
    string Name,
    Guid ProjectId,
    Guid TaskTypeId,
    Guid TaskStatusId,
    DateTimeOffset CreatedDate,
    string? Description = null,
    Guid? ReporterId = null,
    DateTimeOffset? UpdatedDate = null,
    DateTimeOffset? DueDate = null,
    DateTimeOffset? ResolvedDate = null);
