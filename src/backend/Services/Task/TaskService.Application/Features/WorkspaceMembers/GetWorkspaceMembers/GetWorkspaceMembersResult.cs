using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Common.DTO;

namespace TaskService.Application.Features.WorkspaceMembers.GetAllUsers
{
    public record GetWorkspaceMembersResult(Guid Id,
                                            Guid KeycloakId,
                                            List<ProjectMemberDto>? ProjectMembers = null,
                                            List<WorkSpaceMemberDto>? WorkSpaceMembers = null);
}
