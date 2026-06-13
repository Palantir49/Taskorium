using TaskService.Contracts.Enum;

namespace TaskService.Contracts.IssueStatus.Request;

public record class IssueStatusUpdateRequest(
    string Name,
    int NumberType,
    int Position,
    string? Color);

