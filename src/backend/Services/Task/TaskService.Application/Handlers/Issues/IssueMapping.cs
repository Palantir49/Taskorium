using TaskService.Application.Handlers.Issues.Command;
using TaskService.Contracts.Issue.Requests;

namespace TaskService.Application.Handlers.Issues
{
    public static class IssueMapping
    {
        public static CreateIssueCommand ToCommand(this CreateIssueRequest request)
        {
            return new CreateIssueCommand(
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
