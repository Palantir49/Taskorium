using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;

namespace TaskService.Application.Features.IssueTypes.Command;

public record class IssueTypeCreateCommand(
    string name,
    Guid projectId,
    string? color) : ICommand<IssueTypeResponse>;
