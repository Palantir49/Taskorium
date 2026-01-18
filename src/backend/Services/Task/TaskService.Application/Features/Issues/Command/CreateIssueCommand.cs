namespace TaskService.Application.Commands.Issues.Command;

public record CreateIssueCommand(string Name,
    Guid ProjectId,
    Guid TaskTypeId,
    Guid TaskStatusId,
    string? Description = null,
    Guid? ReporterId = null,
    DateTimeOffset? DueDate = null);
