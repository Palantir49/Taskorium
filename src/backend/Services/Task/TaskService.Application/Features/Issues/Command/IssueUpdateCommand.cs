using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Issues.Command;

public record class IssueUpdateCommand(
    Guid Id,
    string Name,
    Guid IssueStatusId,
    int NumberIssueType,
    IReadOnlyCollection<IssueAssigneesDto> Assignees,
    string? Description = null,
    DateTimeOffset? DueDate = null,
    IReadOnlyCollection<Attachment>? Attachments = null) : ICommand<IssueResponse>;
