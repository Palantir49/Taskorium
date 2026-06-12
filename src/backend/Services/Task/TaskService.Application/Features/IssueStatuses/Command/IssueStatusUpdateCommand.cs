using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;
using TaskService.Contracts.IssueStatus;

namespace TaskService.Application.Features.IssueStatuses.Command;

public record class IssueStatusUpdateCommand(
    Guid Id,
    string Name,
    IssueStatusTypeDto Type,
    int Position,
    string? Color) : ICommand<IssueStatusResponse>;
