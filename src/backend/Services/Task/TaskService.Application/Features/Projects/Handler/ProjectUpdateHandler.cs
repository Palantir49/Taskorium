using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectUpdateHandler(IRepositoryWrapper wrapper) : IRequestHandler<ProjectUpdateCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectUpdateCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await wrapper.Projects.GetByIdAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.id} не найдена");

        project.UpdateName(request.Name);
        project.UpdateDescription(request.Description);

        await wrapper.Projects.UpdateAsync(project, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);

        return project.ToResponse();
    }
}
