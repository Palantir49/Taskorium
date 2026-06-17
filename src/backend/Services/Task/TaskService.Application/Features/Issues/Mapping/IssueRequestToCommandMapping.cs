using TaskService.Application.Commands.Issues.Query;
using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Features.Issues.Command;
using TaskService.Contracts.Enum;
using TaskService.Contracts.Issue.Requests;

namespace TaskService.Application.Features.Issues.Mapping;

//TODO issue assignees
public static class IssueRequestToCommandMapping
{
    public static IssueCreateCommand ToCommand(this IssueCreateRequest request)
    {
        var attachments = request.Attachments?
            .Select(file => new AttachmentDto
            {
                Content = file.OpenReadStream(),
                ContentType = file.ContentType,
                ContentLength = file.Length,
                Name = file.FileName
            })
            .ToList();

        return new IssueCreateCommand(
            request.Name,
            request.ProjectId,
            (IssueTypeDto)request.NumberIssueType,
            (IssuePriorityDto)request.NumberIssuePriority,
            request.Description,
            NormalizeDueDate(request.DueDate),
            attachments,
            request.Assignees
        );
    }

    public static IssueUpdateCommand CreateUpdateCommand(Guid id, UpdateIssueRequest request)
    {
        return new IssueUpdateCommand(
            id,
            request.Name,
            request.IssueStatusId,
            Description: request.Description,
            NumberIssueType: request.NumberIssueType,
            DueDate: NormalizeDueDate(request.DueDate),
            Assignees: request.Assignees
        );
    }

    private static DateTimeOffset? NormalizeDueDate(DateTimeOffset? dueDate)
    {
        if (!dueDate.HasValue)
        {
            return null;
        }

        var value = dueDate.Value;
        return new DateTimeOffset(
            value.Year,
            value.Month,
            value.Day,
            0,
            0,
            0,
            TimeSpan.Zero);
    }

    public static GetAllIssuesQuery ToCommand(this GetIssuesRequest reques)
    {
        return new GetAllIssuesQuery();
    }
}
