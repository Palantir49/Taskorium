using TaskService.Application.Commands.Projects.Command;
using TaskService.Application.Features.Projects.Command;
using TaskService.Contracts.Project.Requests;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;

namespace TaskService.Application.Commands.Projects;

public static class ProjectMapping
{
    public static ProjectCreateCommand ToCommand(this CreateProjectRequest request)
    {
        return new ProjectCreateCommand(
            Name: request.Name,
            Description: request.Description,
            Abbreviation: request.Abbreviation,
            WorkspaceId: request.WorkspaceId
            );
    }

    public static ProjectResponse ToResponse(this Project project)
    {
        return new ProjectResponse(
            Id: project.Id,
            Name: project.Name.ToString(),
            Description: project.Description,
            Abbreviation: project.Abbreviation,
            WorkspaceId: project.WorkspaceId,
            CreatedDate: project.CreatedDate);
    }

    public static ProjectUpdateCommand ProjectUpdateCommand(Guid id, UpdateProjectRequest request)
    {
        return new ProjectUpdateCommand(
            id: id,
            Name: request.Name,
            Description: request.Description);
    }
}
