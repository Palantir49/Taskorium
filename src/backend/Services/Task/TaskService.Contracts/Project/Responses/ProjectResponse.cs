using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Project.Responses
{
    public record ProjectResponse(
        Guid Id,
        string Name,
        string? Description,
        Guid WorkspaceId,
        DateTimeOffset CreatedDate);
}
