using TaskService.Application.Commands.Issues.Command;
using TaskService.Application.Commands.Workspaces.Create;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Issues.Handler;

public class IssueCreateHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueCreateCommand, IssueResponse>
{

    public async Task<IssueResponse> Handle(IssueCreateCommand request, CancellationToken cancellationToken = default)
    {
        var project = await wrapper.Projects.GetByIdAsync(request.ProjectId);
        //TODO: проверка существования статуса
        //TODO: проверка существования типа

        if (project == null)
        {
            throw new InvalidOperationException("Project not found.");
        }

        var issue = Issue.Create(
            name: request.Name,
            description: request.Description,
            projectId: request.ProjectId,
            taskTypeId: request.TaskTypeId,
            taskStatusId: request.TaskStatusId,
            dueDate: request.DueDate
        );
        await wrapper.Issues.AddAsync(issue, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);

        return issue.ToResponce();
    }
}
