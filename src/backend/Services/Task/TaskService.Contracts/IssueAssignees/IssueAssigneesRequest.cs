namespace TaskService.Contracts.IssueAssignees;

public record IssueAssigneesRequest(Guid UserId, int Role);
