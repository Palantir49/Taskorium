using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;
using TaskService.Contracts.User.Responses;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Application.Features.Users.Read.GetUserWorkspacesById
{
    public record GetUserWorkspacesByIdResult(ICollection<UsersWorkspaceResponse> UsersWorkspaces);

}
