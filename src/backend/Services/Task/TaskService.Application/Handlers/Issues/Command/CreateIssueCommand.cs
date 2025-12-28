namespace TaskService.Application.Handlers.Issues.Command;

public record CreateIssueCommand(string Name,
    Guid ProjectId,
    Guid TaskTypeId,
    Guid TaskStatusId,
    string? Description = null,
    Guid? ReporterId = null,
    DateTimeOffset? DueDate = null);
