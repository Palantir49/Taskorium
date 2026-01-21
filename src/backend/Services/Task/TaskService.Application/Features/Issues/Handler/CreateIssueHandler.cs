using TaskService.Application.Commands.Issues.Command;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Issues.Handler;

public class CreateIssueHandler
{
    private readonly IRepositoryWrapper _wrapper;

    public CreateIssueHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<IssueResponse> HandleAsync(CreateIssueCommand command, CancellationToken ct = default)
    {
        var project = await _wrapper.Projects.GetByIdAsync(command.ProjectId);
        //TODO: проверка существования статуса
        //TODO: проверка существования типа
        //TODO: проверка существования юзера

        if (project == null)
        {
            throw new InvalidOperationException("Project not found.");
        }

        var issue = Issue.Create(
            name: command.Name,
            description: command.Description,
            projectId: command.ProjectId,
            taskTypeId: command.TaskTypeId,
            taskStatusId: command.TaskStatusId,
            dueDate: command.DueDate
        );
        await _wrapper.Issues.AddAsync(issue, ct);
        await _wrapper.SaveChangesAsync(ct);

        return new IssueResponse(Id: issue.Id, Name: issue.Name.ToString(), ProjectId: issue.ProjectId, TaskTypeId: issue.IssueTypeId, TaskStatusId: issue.IssueStatusId,
            CreatedDate: issue.CreatedDate, Description: issue.Description, UpdatedDate: issue.UpdatedDate, DueDate: issue.DueDate,
            ResolvedDate: issue.ResolvedDate);
    }
}
