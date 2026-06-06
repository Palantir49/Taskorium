using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.IssueAssignee.UpdateIssueAssignee;

public record UpdateIssueAssigneeCommand(Guid IssueId, Guid UserId, AssigneesRoles Role) : ICommand<IssueAssigneesResponce>;
