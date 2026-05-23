using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Features.Projects.Write.AddProjectMember;
using TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;

namespace TaskService.Application.Features.ProjectMembers.Write.UpdateProjectMemberRole
{
    public record UpdateProjectMemberRoleCommand(Guid ProjectId, Guid UserId, ProjectRolesDto NewRole) : ICommand<AddProjectMemberResult>;

}
