using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Common.DTO
{
    public record ProjectMemberDto(Guid ProjectId, Guid UserId, RolesDto Role);
}
