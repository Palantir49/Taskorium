namespace TaskService.Contracts.IssueAssignees;

public record IssueAssigneesResponce(Guid IssueId, Guid UserId, int Role);
