using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Common.DTO;

namespace TaskService.Application.Features.WorkspaceMembers.Read.GetWorkspaceMembersHandler
{
    public record GetWorkspaceMembersByKeycloakIdResult(Guid Id,
                                            Guid KeycloakId,
                                            List<ProjectMemberDto>? ProjectMembers = null,
                                            List<WorkSpaceMemberDto>? WorkSpaceMembers = null);
}
