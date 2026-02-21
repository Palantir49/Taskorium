using TaskService.Application.Commands.Issues.Command;
using TaskService.Application.Commands.Issues.Query;
using TaskService.Application.Features.Issues.Command;
using TaskService.Contracts.Issue.Requests;

namespace TaskService.Application.Features.Issues.Mapping
{
    public static class IssueRequestToCommandMapping
    {
        public static IssueCreateCommand ToCommand(this IssueCreateRequest request)
        {
            return new IssueCreateCommand(
                Name: request.Name,
                ProjectId: request.ProjectId,
                IssueTagId: request.IssueTagId,
                IssueStatusId: request.IssueStatusId,
                Description: request.Description,
                numberIssueType: request.numberIssueType,
                DueDate: request.DueDate
                );
        }

        public static IssueUpdateCommand CreateUpdateCommand(Guid id, UpdateIssueRequest request)
        {
            return new IssueUpdateCommand(
                id: id,
                Name: request.Name,
                IssueTagId: request.IssueTagId,
                IssueStatusId: request.IssueStatusId,
                Description: request.Description,
                numberIssueType: request.numberIssueType,
                DueDate: request.DueDate
                );
        }

        public static GetAllIssuesQuery ToCommand(this GetIssuesRequest reques)
        {
            return new GetAllIssuesQuery();
        }
    }
}
