using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;

namespace TaskService.Application.Features.IssueAssignee.GetIssueAssigneesByIssueId
{
    public record GetIssueAssigneesByIssueIdQuery(Guid IssueId) : IQuery<IEnumerable<IssueAssigneesResponce>>;
}
