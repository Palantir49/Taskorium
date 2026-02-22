using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Common.DTO
{
    public record WorkSpaceMemberDto(Guid WorkspaceId, Guid UserId, RoleDto RoleDto);
}
