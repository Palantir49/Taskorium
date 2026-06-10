using TaskService.Application.Features.Projects.Write.UpdateProject;
using TaskService.Application.Mapping;
using TaskService.Contracts.Project.Requests;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Projects;

public static class ProjectMapping
{
    //public static ProjectCreateCommand ToCommand(this CreateProjectRequest request)
    //{
    //    return new ProjectCreateCommand(
    //        Name: request.Name,
    //        Description: request.Description,
    //        Abbreviation: request.Abbreviation,
    //        WorkspaceId: request.WorkspaceId
    //        );
    //}

    public static ProjectResponse ToResponse(this Project project, Guid userId)
    {
        return new ProjectResponse(
            project.Id,
            project.Name.ToString(),
            project.Description,
            project.Abbreviation,
            project.WorkspaceId,
            project.CreatedDate,
            project.ProjectMembers.First(element => element.UserId == userId).Role.ToDto());
    }

    public static UpdateProjectCommand ProjectUpdateCommand(Guid id, UpdateProjectRequest request)
    {
        return new UpdateProjectCommand(
            id,
            request.Name,
            request.Description);
    }
}
