using TaskService.Application.Handlers.Issues.Command;
using TaskService.Application.Wrapper;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;

namespace TaskService.Application.Handlers.Issues.Handler;

public class CreateIssueHandler
{
    private readonly IRepositoryWrapper _wrapper;

    public CreateIssueHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<IssueResponse> HandleAsync(CreateIssueCommand command, CancellationToken ct = default)
    {
        //FAQ: нужно ли тут делать проверки, если у нас есть ограничения на уровне базы данных
        var project = await _wrapper.Projects.GetByIdAsync(command.ProjectId);
        //TODO: проверка существования статуса
        //TODO: проверка существования типа
        //TODO: проверка существования юзера

        if (project == null)
        {//FAQ: как я понимаю нужно создавать свои исключения типа NotFoundException?
            throw new Exception("Project not found.");
        }

        var issue = Issue.Create(
            name: command.Name,
            description: command.Description,
            projectId: command.ProjectId,
            taskTypeId: command.TaskTypeId,
            taskStatusId: command.TaskStatusId,
            reporterId: command.ReporterId,
            dueDate: command.DueDate
        );
        await _wrapper.Issues.AddAsync(issue, ct);
        await _wrapper.SaveChangesAsync(ct);

        return new IssueResponse(Id:issue.Id, Name: issue.Name,ProjectId:issue.ProjectId, TaskTypeId:issue.TaskTypeId, TaskStatusId:issue.TaskStatusId, 
            CreatedDate:issue.CreatedDate, Description:issue.Description,ReporterId:issue.ReporterId, UpdatedDate:issue.UpdatedDate, DueDate:issue.DueDate, 
            ResolvedDate:issue.ResolvedDate);
    }
}
