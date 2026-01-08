using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Commands.Projects.Command
{
    public record CreateProjectCommand(
    string Name,
    string Description,
    Guid WorkspaceId);
}
