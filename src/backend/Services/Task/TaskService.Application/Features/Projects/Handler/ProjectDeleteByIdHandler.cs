using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectDeleteByIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<ProjectDeleteByIdCommand, int>
{
    public async Task<int> Handle(ProjectDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await wrapper.Projects.GetByIdAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.id} не найдена");
        List<Issue> issues = await wrapper.Issues.GetByProjectIdAsync(projectId: project.Id, cancellationToken);
        if (issues != null && issues.Count > 0)
            throw new NullReferenceException($"Нельзя удалить статус, пока существуют связанные задачи");
        //TODO: изменить исключение на подходящее

        await wrapper.Projects.DeleteAsync(project, cancellationToken);
        return await wrapper.SaveChangesAsync(cancellationToken);
    }
}
