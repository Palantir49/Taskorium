using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;

namespace TaskService.Application.Features.IssueStatuses.Command;

public record class StatusGetByIdQuery(Guid id) : IQuery<IssueStatusResponse>;
