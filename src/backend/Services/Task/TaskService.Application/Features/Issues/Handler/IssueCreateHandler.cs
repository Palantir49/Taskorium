using Microsoft.EntityFrameworkCore;
using TaskService.Application.Commands.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Commands.Issues.Handler;

public class IssueCreateHandler(TaskServiceDbContext context) : IRequestHandler<IssueCreateCommand, IssueResponse>
{
    public async Task<IssueResponse> Handle(IssueCreateCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await context.Projects.FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.ProjectId} не найдена");

        IssueStatus status = await context.IssueStatus.FindAsync(request.ProjectId, IssueStatusType.Initial, cancellationToken) ??
            throw new NullReferenceException($"Не найден статус инициализации задачи для проекта {request.ProjectId}");

        int countIssue = await context.Issues.CountAsync(x=> x.ProjectId == project.Id, cancellationToken);

        string issueKey = $"{project.Abbreviation}-{countIssue + 1}";

        var issue = Issue.Create(
            name: request.Name,
            description: request.Description,
            key: issueKey,
            projectId: request.ProjectId,
            taskStatusId: status.Id,
            numberIssueType: request.numberIssueType,
            dueDate: request.DueDate
        );
        await context.Issues.AddAsync(issue, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return issue.ToResponse();
    }
}
