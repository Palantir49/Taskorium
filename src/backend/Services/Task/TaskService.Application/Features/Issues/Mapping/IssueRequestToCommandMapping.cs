using TaskService.Application.Commands.Issues.Query;
using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Features.Issues.Command;
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
            request.NumberIssueType,
            request.NumberIssuePriority,
            request.Description,
            request.DueDate?.ToUniversalTime(),
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
            DueDate: request.DueDate?.ToUniversalTime(),
            Assignees: request.Assignees
        );
    }

    public static GetAllIssuesQuery ToCommand(this GetIssuesRequest reques)
    {
        return new GetAllIssuesQuery();
    }
}
