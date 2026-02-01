using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;

namespace TaskService.Application.Features.IssueTypes.Command;

public record class IssueTypeUpdateCommand(
    Guid id,
    string name,
    string? color) : ICommand<IssueTypeResponse>;
