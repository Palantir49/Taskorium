using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;

namespace TaskService.Application.Features.IssueStatuses.Command;

public record class StatusGetByProjectIdQuery(Guid id) : IQuery<IssueStatusResponse>;
