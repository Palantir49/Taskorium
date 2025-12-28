using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Handlers.Projects.Command;
using TaskService.Contracts.Project.Requests;

namespace TaskService.Application.Handlers.Projects
{
    public static class ProjectMapping
    {
        public static CreateProjectCommand ToCommand(this CreateProjectRequest request)
        {
            return new CreateProjectCommand(
                Name: request.Name,
                Description:request.Description,
                WorkspaceId: request.WorkspaceId
                );
        }
    }
}
