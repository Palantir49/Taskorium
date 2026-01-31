using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Projects.Handler;

public  class ProjectUpdateHandler(IRepositoryWrapper wrapper) : IRequestHandler<ProjectUpdateCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectUpdateCommand request, CancellationToken cancellationToken = default)
    {
        //throw new NullReferenceException($"Задача с id: {request.id} не найдена");

        Project project = await wrapper.Projects.GetByIdAsync(request.id) ?? throw new NullReferenceException($"Проект с id: {request.id} не найдена");

        project.UpdateName(request.Name);
        project.UpdateDescription(request.Description);

        await wrapper.Projects.UpdateAsync(project);
        await wrapper.SaveChangesAsync();

        return project.ToResponse();
    }
}
