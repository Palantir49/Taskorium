using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Application.Features.Workspaces.Read.GetWorkspaceMembers
{
    public record GetWorkspaceMembersQuery(Guid Id) : IRequest<WorkspaceMembersResponse>;
}
