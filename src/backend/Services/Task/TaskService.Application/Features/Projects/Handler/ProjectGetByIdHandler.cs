using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

internal class ProjectGetByIdHandler(TaskServiceDbContext context) : IRequestHandler<ProjectGetByIdQuery, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        Project? project = await context.Projects.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.id} не найдена");

        return project.ToResponse();
    }
}
