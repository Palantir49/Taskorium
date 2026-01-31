using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Features.Issues.Command;

public record class IssueUpdateCommand(
    Guid id,
    string Name,
    Guid ProjectId,
    Guid TaskTypeId,
    Guid TaskStatusId,
    string? Description = null,
    Guid? ReporterId = null,
    DateTimeOffset? DueDate = null) : ICommand<IssueResponse>;
