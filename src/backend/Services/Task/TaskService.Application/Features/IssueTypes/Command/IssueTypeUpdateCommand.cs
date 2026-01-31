using TaskService.Application.Mediator;

namespace TaskService.Application.Features.IssueTypes.Command;

public record class IssueTypeUpdateCommand(
    Guid id,
    string name,
    string color) : ICommand<int>;
