using TaskService.Contracts.Attachment;
using TaskService.Contracts.Collections;
using TaskService.Contracts.Common.DTO;

namespace TaskService.Contracts.Issue.Responses;

public record IssueResponse(
    Guid Id,
    string Name,
    string? Description,
    Guid ProjectId,
    Guid TaskStatusId,
    IssueTypeResponse IssueType,
    IssuePriorityResponse IssuePriority,
    DateTimeOffset CreatedDate,
    DateTimeOffset? UpdatedDate,
    DateTimeOffset? DueDate,
    DateTimeOffset? ResolvedDate,
    IEnumerable<IssueAssigneesDto>? Assignees,
    IEnumerable<AttachmentResponce>? Attachments);
