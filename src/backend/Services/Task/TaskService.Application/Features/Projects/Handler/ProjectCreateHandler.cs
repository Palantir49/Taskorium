using TaskService.Application.Commands.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Commands.Projects.Handler;

public class ProjectCreateHandler(TaskServiceDbContext context) : IRequestHandler<ProjectCreateCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectCreateCommand request, CancellationToken cancellationToken = default)
    {
        var workspace = await context.Workspaces.FindAsync(request.WorkspaceId, cancellationToken) ??
            throw new NullReferenceException($"Рабочая область с id: {request.WorkspaceId} не найдена");

        var project = Project.Create(
            name: request.Name,
            description: request.Description,
            workspaceId: request.WorkspaceId
        );
        //TODO: добавить создание статусов и типа
        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return project.ToResponse();
    }
}
