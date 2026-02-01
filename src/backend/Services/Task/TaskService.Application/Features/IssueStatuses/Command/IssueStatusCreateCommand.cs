using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;

namespace TaskService.Application.Features.IssueStatuses.Command;

public record class IssueStatusCreateCommand(
    string name,
    Guid projectId,
    string type,
    int position,
    string? color) : ICommand<IssueStatusResponse>;
