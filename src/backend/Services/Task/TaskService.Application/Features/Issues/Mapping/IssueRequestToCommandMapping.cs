using TaskService.Application.Commands.Issues.Query;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Dto;
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
            request.DueDate,
            attachments
        );
    }

    public static IssueUpdateCommand CreateUpdateCommand(Guid id, UpdateIssueRequest request)
    {
        return new IssueUpdateCommand(
            id,
            request.Name,
            request.IssueStatusId,
            Description: request.Description,
            numberIssueType: request.NumberIssueType,
            DueDate: request.DueDate
        );
    }

    public static GetAllIssuesQuery ToCommand(this GetIssuesRequest reques)
    {
        return new GetAllIssuesQuery();
    }
}
