using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;

namespace TaskService.Application.Features.IssueStatuses.Command;

public record class IssueStatusGetByIdQuery(Guid id) : IQuery<IssueStatusResponse>;
