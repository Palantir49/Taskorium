using TaskService.Application.Mediator;

namespace TaskService.Application.Features.IssueStatuses.Command;

public record class IssueStatusDeleteByIdCommand(Guid id) : ICommand<int>;
