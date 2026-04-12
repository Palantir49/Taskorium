using Microsoft.AspNetCore.Http;

namespace TaskService.Contracts.Issue.Requests;

//TODO issue assignees
public record IssueCreateRequest(
    string Name,
    Guid ProjectId,
    int NumberIssueType,
    int NumberIssuePriority,
    string? Description = null,
    DateTimeOffset? DueDate = null,
    List<IFormFile>? Attachments = null);
