using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Features.Issues.Command;

public record class IssueUpdateCommand(
    Guid id,
    string Name,
    Guid IssueTagId,
    Guid IssueStatusId,
    int numberIssueType,
    string? Description = null,
    DateTimeOffset? DueDate = null) : ICommand<IssueResponse>;
