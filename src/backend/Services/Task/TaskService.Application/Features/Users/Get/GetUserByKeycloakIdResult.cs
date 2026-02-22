using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Common.DTO;

namespace TaskService.Application.Features.Users.Get
{
    public record GetUserByKeycloakIdResult(
     Guid Id,
     Guid KeycloakId,
     List<ProjectMemberDto>? ProjectMembers = null,
     List<WorkSpaceMemberDto>? WorkSpaceMembers = null);
}
