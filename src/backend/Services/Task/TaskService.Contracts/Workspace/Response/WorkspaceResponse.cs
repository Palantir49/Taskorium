using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Workspace.Response
{
    public record class WorkspaceResponse(Guid Id, string name, DateTimeOffset createdDate, Guid? ownerId);
}
