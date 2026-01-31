using TaskService.Application.Mediator;

namespace TaskService.Application.Features.IssueStatuses.Command;

public record class IssueStatusUpdateCommand(
    Guid id,
    string name,
    string type,
    int position,
    string? color) : ICommand<int>;
