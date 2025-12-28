namespace TaskService.Contracts.Issue.Requests;

public record class CreateIssueRequest(
    string Name,
    Guid ProjectId,
    Guid TaskTypeId,
    Guid TaskStatusId,
    string? Description = null,
    Guid? ReporterId = null,
    DateTimeOffset? DueDate = null);
