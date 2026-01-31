using TaskService.Application.Commands.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Projects.Handler;

public class ProjectCreateHandler(IRepositoryWrapper wrapper) : IRequestHandler<ProjectCreateCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectCreateCommand request, CancellationToken cancellationToken = default)
    {
        var workspace = await wrapper.Workspaces.GetByIdAsync(request.WorkspaceId);

        if (workspace == null)
        {
            throw new Exception("Project not found.");
        }

        var project = Project.Create(
            name: request.Name,
            description: request.Description,
            workspaceId: request.WorkspaceId
        );
        await wrapper.Projects.AddAsync(project, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);

        return new ProjectResponse(Id: project.Id, Name: project.Name.ToString(), Description: project.Description, WorkspaceId: project.WorkspaceId, CreatedDate: project.CreatedDate);
    }
}
