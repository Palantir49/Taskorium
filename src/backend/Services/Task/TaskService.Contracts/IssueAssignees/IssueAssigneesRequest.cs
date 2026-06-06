using TaskService.Contracts.Enum;

namespace TaskService.Contracts.IssueAssignees;

public record IssueAssigneesRequest(Guid UserId, AssigneesRolesDto Role);
