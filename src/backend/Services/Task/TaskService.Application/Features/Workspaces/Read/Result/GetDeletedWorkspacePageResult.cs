using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Application.Features.Workspaces.Read.Result
{
    public record GetDeletedWorkspacePageResult(IEnumerable<DeletedWorkspaceResponse> workspaces);
}
