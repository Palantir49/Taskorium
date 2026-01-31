using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Commands.Issues.Command;

public record IssueCreateCommand(string Name,
    Guid ProjectId,
    Guid TaskTypeId,
    Guid TaskStatusId,
    string? Description = null,
    Guid? ReporterId = null,
    DateTimeOffset? DueDate = null) : ICommand<IssueResponse>;
