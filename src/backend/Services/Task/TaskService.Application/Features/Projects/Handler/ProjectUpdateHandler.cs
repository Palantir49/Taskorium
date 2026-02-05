using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectUpdateHandler(TaskServiceDbContext context) : IRequestHandler<ProjectUpdateCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectUpdateCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await context.Projects.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.id} не найдена");

        project.UpdateName(request.Name);
        project.UpdateDescription(request.Description);
        context.Projects.Update(project);
        await context.SaveChangesAsync(cancellationToken);

        return project.ToResponse();
    }
}
