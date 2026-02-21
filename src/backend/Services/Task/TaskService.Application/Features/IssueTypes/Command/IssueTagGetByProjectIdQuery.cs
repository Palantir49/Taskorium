using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;

namespace TaskService.Application.Features.IssueTags.Command;

public record class IssueTagGetByProjectIdQuery(Guid id) : IQuery<IEnumerable<IssueTagResponse>>;
