using TaskService.Contracts.Enum;

namespace TaskService.Contracts.IssueAssignees;

public record IssueAssigneesResponce(Guid IssueId, Guid UserId, AssigneesRolesDto Role);
