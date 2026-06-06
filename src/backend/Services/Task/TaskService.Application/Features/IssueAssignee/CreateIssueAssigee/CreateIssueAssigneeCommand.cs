using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;

namespace TaskService.Application.Features.IssueAssignee.CreateIssueAssigee;

public record CreateIssueAssigneeCommand(Guid IssueId, Guid UserId, int Role) : ICommand<IssueAssigneesResponce>;
