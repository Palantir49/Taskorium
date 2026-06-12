using TaskService.Contracts.Enum;

namespace TaskService.Contracts.IssueStatus.Request;

public record class IssueStatusUpdateRequest(
    string Name,
    IssueStatusTypeDto Type,
    int Position,
    string? Color);

