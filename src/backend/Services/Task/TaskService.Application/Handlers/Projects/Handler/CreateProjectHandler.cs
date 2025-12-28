using TaskService.Application.Handlers.Projects.Command;
using TaskService.Application.Wrapper;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;

namespace TaskService.Application.Handlers.Projects.Handler;

public class CreateProjectHandler
{
    private readonly IRepositoryWrapper _wrapper;

    public CreateProjectHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<ProjectResponse> HandleAsync(CreateProjectCommand command, CancellationToken ct = default)
    {
        var workspace = await _wrapper.Workspaces.GetByIdAsync(command.WorkspaceId);

        if (workspace == null)
        {
            throw new Exception("Project not found.");
        }

        var project = Project.Create(
            name: command.Name,
            description: command.Description,
            workspaceId: command.WorkspaceId
        );
        await _wrapper.Projects.AddAsync(project, ct);
        await _wrapper.SaveChangesAsync(ct);

        return new ProjectResponse(Id: project.Id, Name: project.Name, Description: project.Description, WorkspaceId: project.WorkspaceId, CreatedDate: project.CreatedDate);
    }
}
