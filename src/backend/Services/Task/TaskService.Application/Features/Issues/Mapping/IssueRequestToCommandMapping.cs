using System.Diagnostics;
using TaskService.Application.Commands.Issues.Command;
using TaskService.Application.Commands.Issues.Query;
using TaskService.Contracts.Issue.Requests;

namespace TaskService.Application.Features.Issues.Mapping
{
    public static class IssueRequestToCommandMapping
    {
        public static IssueCreateCommand ToCommand(this CreateIssueRequest request)
        {
            return new IssueCreateCommand(
                Name: request.Name,
                ProjectId: request.ProjectId,
                IssueTypeId: request.TaskTypeId,
                IssueStatusId: request.TaskStatusId,
                Description: request.Description,
                DueDate: request.DueDate
                );
        }

        public static IssueUpdateCommand CreateUpdateCommand(Guid id, UpdateIssueRequest request)
        {
            return new IssueUpdateCommand(
                id: id,
                Name: request.Name,
                IssueTypeId: request.IssueTypeId,
                IssueStatusId: request.IssueStatusId,
                Description: request.Description,
                DueDate: request.DueDate
                );
        }

        public static GetAllIssuesQuery ToCommand(this GetIssuesRequest request)
        {
            return new GetAllIssuesQuery();
        }
    }
}
