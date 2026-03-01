using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Commands.Issues.Command;

public record IssueCreateCommand(
    string Name,
    Guid ProjectId,
    //Guid IssueTagId,
    //Guid IssueStatusId,
    int NumberIssueType,
    int NumberIssuePriority,
    string? Description = null,
    DateTimeOffset? DueDate = null) : ICommand<IssueResponse>;
