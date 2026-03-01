using TaskService.Contracts.Collections;

namespace TaskService.Contracts.Issue.Responses;

public record IssueResponse(
    Guid Id,
    string Name,
    string? Description,
    Guid ProjectId,
    Guid TaskStatusId,
    IssueTypeResponse IssueType,
    IssuePriorityResponse IssuePriority,
    DateTimeOffset CreatedDate,
    DateTimeOffset? UpdatedDate,
    DateTimeOffset? DueDate,
    DateTimeOffset? ResolvedDate);
