using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectDeleteByIdHandler(TaskServiceDbContext context) : IRequestHandler<ProjectDeleteByIdCommand, int>
{
    public async Task<int> Handle(ProjectDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await context.Projects.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.id} не найдена");
        List<Issue> issues = await context.Issues.Where(x => x.ProjectId == project.Id).ToListAsync();
        if (issues != null && issues.Count > 0)
            throw new NullReferenceException($"Нельзя удалить статус, пока существуют связанные задачи");
        //TODO: изменить исключение на подходящее

        List<IssueStatus> statuses = await context.IssueStatus.Where(x => x.ProjectId == project.Id).ToListAsync();
        foreach (IssueStatus status in statuses)
            context.IssueStatus.Remove(status);

        List<IssueType> types = await context.IssueType.Where(x => x.ProjectId == project.Id).ToListAsync();

        foreach (IssueType type in types)
            context.IssueType.Remove(type);

        context.Projects.Remove(project);
        return await context.SaveChangesAsync(cancellationToken);
    }
}
