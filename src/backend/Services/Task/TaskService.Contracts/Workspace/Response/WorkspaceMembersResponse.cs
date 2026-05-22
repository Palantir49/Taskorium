using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Common.DTO;

namespace TaskService.Contracts.Workspace.Response
{
    public record WorkspaceMembersResponse(Guid WorkspaceId, string WorkspaceName, IEnumerable<WorkspaceUserDto> Members);

}
