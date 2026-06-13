using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Application.Features.Workspaces.Read.GetDeletedWorkspace
{
    public record GetDeletedWorkspacePageResult(IEnumerable<DeletedWorkspaceResponse> Workspaces);
}
