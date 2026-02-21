using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;

namespace TaskService.Application.Features.IssueTags.Command;

public record class IssueTagGetByIdQuery(Guid id) : IQuery<IssueTagResponse>;
