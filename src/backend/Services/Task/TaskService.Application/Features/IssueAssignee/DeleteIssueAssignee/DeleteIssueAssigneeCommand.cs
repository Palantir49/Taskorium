using TaskService.Application.Mediator;

namespace TaskService.Application.Features.IssueAssignee.DeleteIssueAssignee
{
    public record DeleteIssueAssigneeCommand(Guid IssueId, Guid UserId) : ICommand<int>;
}
