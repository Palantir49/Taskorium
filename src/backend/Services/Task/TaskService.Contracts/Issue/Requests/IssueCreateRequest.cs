using Microsoft.AspNetCore.Http;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Issue.Requests;

public record IssueCreateRequest(
    string Name,
    Guid ProjectId,
    IssueTypeDto IssueType,
    IssuePriorityDto IssuePriority,
    string? Description = null,
    DateTimeOffset? DueDate = null,
    List<IFormFile>? Attachments = null,
    List<IssueAssigneesDto>? Assignees = null);
