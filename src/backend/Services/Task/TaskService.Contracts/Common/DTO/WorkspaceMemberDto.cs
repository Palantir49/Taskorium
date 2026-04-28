using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Common.DTO
{
    public record WorkSpaceMemberDto(Guid WorkspaceId, Guid UserId, RolesDto Role);
}
