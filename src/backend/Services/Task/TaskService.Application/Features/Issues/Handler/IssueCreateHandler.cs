using TaskService.Application.Commands.Issues.Command;
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
        //TODO: проверка существования типа

        if (project == null)
        {
            throw new NullReferenceException($"Проект с id: {request.IssueStatusId} не найдена");
        }

        IssueStatus? status = await wrapper.IssueStatus.GetByIdAsync(request.IssueStatusId);

        if (status == null)
            throw new NullReferenceException($"Статус задачи с id: {request.IssueStatusId} не найдена");

        //TODO: проверить что можно создавать с этим статусом

        IssueType? type = await wrapper.IssueType.GetByIdAsync(request.IssueTypeId);

        if (type == null)
            throw new NullReferenceException($"Тип задачи с id: {request.IssueTypeId} не найдена");

        //TODO: проверить что можно создавать с этим типом

        var issue = Issue.Create(
            name: request.Name,
            description: request.Description,
            projectId: request.ProjectId,
            taskTypeId: request.IssueTypeId,
            taskStatusId: request.IssueStatusId,
            dueDate: request.DueDate
        );
        await wrapper.Issues.AddAsync(issue, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);

        return issue.ToResponce();
    }
}
