using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;

namespace TaskService.Application.Features.IssueAssignee.CreateIssueAssigee;

public record CreateIssueAssigneesCommand(Guid IssueId, Guid UserId, int role) : ICommand<IssueAssigneesResponce>;
