using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Features.Issues.Command;

public record IssueCreateCommand(
    string Name,
    Guid ProjectId,
    int NumberIssueType,
    int NumberIssuePriority,
    string? Description = null,
    DateTimeOffset? DueDate = null,
    List<AttachmentDto>? AttachmentDtos = null,
    List<IssueAssigneesDto>? AssigneeDtos = null) : ICommand<IssueResponse>;
