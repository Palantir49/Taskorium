using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;

namespace TaskService.Application.Features.IssueAssignee.UpdateIssueAssignee;

public record UpdateIssueAssigneeCommand(Guid IssueId, Guid UserId, int Role) : ICommand<IssueAssigneesResponce>;
