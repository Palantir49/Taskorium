using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Issues.Command;

public record AddTagToIssueCommand(
    Guid IssueId,
    Guid TagId
    ) : ICommand<bool>;

