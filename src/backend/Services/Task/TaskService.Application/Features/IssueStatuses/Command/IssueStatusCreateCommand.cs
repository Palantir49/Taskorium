using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;
using TaskService.Contracts.IssueStatus;

namespace TaskService.Application.Features.IssueStatuses.Command;

public record class IssueStatusCreateCommand(
    string Name,
    Guid ProjectId,
    IssueStatusTypeDto Type,
    int Position,
    string Color) : ICommand<IssueStatusResponse>;
