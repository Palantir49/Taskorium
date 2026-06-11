using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Enum;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Features.Issues.Command;

public record IssueCreateCommand(
    string Name,
    Guid ProjectId,
    IssueTypeDto IssueType,
    IssuePriorityDto IssuePriority,
    string? Description = null,
    DateTimeOffset? DueDate = null,
    List<AttachmentDto>? AttachmentDtos = null,
    List<IssueAssigneesDto>? AssigneeDtos = null) : ICommand<IssueResponse>;
