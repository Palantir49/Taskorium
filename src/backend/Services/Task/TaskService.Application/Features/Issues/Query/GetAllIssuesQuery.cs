using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Commands.Issues.Query;

public record GetAllIssuesQuery() : IQuery<IssuesResponse>;
