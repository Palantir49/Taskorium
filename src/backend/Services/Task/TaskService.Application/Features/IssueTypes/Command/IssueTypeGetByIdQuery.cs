using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;

namespace TaskService.Application.Features.IssueTypes.Command;

public record class IssueTypeGetByIdQuery(Guid id) : IQuery<IssueTypeResponse>;
