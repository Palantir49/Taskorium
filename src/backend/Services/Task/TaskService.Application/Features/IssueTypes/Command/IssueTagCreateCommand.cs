using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;

namespace TaskService.Application.Features.IssueTags.Command;

public record class IssueTagCreateCommand(
    string name,
    Guid projectId) : ICommand<IssueTagResponse>;
