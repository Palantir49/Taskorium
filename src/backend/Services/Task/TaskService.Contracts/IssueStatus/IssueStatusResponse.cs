
namespace TaskService.Contracts.IssueStatus;

public record class IssueStatusResponse(
    Guid id,
    string name,
    Guid projectId,
    string type,
    int position,
    string? color);
