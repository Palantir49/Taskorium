using TaskService.Application.Commands.Issues.Command;
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
                TaskTypeId: request.TaskTypeId,
                TaskStatusId: request.TaskStatusId,
                Description: request.Description,
                ReporterId: request.ReporterId,
                DueDate: request.DueDate
                );
        }
    }
}
