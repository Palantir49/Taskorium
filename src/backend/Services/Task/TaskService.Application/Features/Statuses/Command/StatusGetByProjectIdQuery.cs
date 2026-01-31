using TaskService.Application.Mediator;
using TaskService.Contracts.Status;

namespace TaskService.Application.Features.Statuses.Command;

public record class StatusGetByProjectIdQuery(Guid id) : IQuery<StatusResponse>;
