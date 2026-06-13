using TaskService.Contracts.Enum;

namespace TaskService.Contracts.IssueStatus.Request;

public record class IssueStatusCreateRequest(
    string Name,
    Guid ProjectId,
    int NumberType,
    int Position,
    string Color);

