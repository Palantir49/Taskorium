using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Features.Issues.Command
{
    public record class IssueGetByIdQuery(Guid id) : IQuery<IssueResponse>;
}
