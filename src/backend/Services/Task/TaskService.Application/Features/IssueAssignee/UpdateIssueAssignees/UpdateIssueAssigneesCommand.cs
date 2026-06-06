using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;

namespace TaskService.Application.Features.IssueAssignee.UpdateIssueAssignees;

public record UpdateIssueAssigneesCommand(Guid IssueId, Guid UserId, int Role) : ICommand<IssueAssigneesResponce>;
