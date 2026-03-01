using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;

namespace TaskService.Application.Features.IssueTags.Command;

public record class IssueTagUpdateCommand(
    Guid id,
    string name) : ICommand<IssueTagResponse>;
