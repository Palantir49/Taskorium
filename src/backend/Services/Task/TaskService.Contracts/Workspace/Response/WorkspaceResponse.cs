using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Workspace.Response
{
    public record class WorkspaceResponse(Guid id, string name, DateTimeOffset createdDate, Guid? ownerId);
}
