using Microsoft.AspNetCore.Http;
using TaskService.Contracts.Common.DTO;

namespace TaskService.Contracts.Issue.Requests;

public record IssueCreateRequest(
    string Name,
    Guid ProjectId,
    int NumberIssueType,
    int NumberIssuePriority,
    string? Description = null,
    DateTimeOffset? DueDate = null,
    List<IFormFile>? Attachments = null,
    List<IssueAssigneesDto>? Assignees = null);
