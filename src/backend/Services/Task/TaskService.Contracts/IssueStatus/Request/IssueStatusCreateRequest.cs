namespace TaskService.Contracts.IssueStatus.Request;

public record class IssueStatusCreateRequest(
    string name,
    Guid projectId,
    int numberType,
    int position,
    string color);

