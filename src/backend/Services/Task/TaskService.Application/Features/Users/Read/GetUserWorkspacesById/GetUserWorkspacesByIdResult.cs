using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Application.Features.Users.Read.GetUserWorkspacesById
{
    public record GetUserWorkspacesByIdResult(ICollection<WorkspaceResponse> Workspaces);

}
