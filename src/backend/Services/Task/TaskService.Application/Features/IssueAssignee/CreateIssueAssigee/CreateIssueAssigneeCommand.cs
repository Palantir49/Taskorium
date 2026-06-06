using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.IssueAssignee.CreateIssueAssigee;

public record CreateIssueAssigneeCommand(Guid IssueId, Guid UserId, AssigneesRoles Role) : ICommand<IssueAssigneesResponce>;
