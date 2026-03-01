using Microsoft.EntityFrameworkCore;
using TaskService.Application.Commands.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Commands.Issues.Handler;

public class IssueCreateHandler(TaskServiceDbContext context) : IRequestHandler<IssueCreateCommand, IssueResponse>
{
    public async Task<IssueResponse> Handle(IssueCreateCommand request, CancellationToken cancellationToken = default)
    {
        var project = await context.Projects.FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.ProjectId} не найдена");

        //TODO: создавать с статусом инициализации из проекта
        IssueStatus? status = await context.IssueStatus.FindAsync(request.IssueStatusId, cancellationToken) ??
            throw new NullReferenceException($"Статус задачи с id: {request.IssueStatusId} не найдена");

        //TODO: проверить что можно создавать с этим статусом

        var issue = Issue.Create(
            name: request.Name,
            description: request.Description,
            key: "",
            projectId: request.ProjectId,
            taskTagId: request.IssueTagId,
            taskStatusId: request.IssueStatusId,
            numberIssueType: request.numberIssueType,
            dueDate: request.DueDate
        );
        await context.Issues.AddAsync(issue, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return issue.ToResponse();
    }
}
