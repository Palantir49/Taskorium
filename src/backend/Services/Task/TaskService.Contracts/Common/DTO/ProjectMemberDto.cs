using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Common.DTO
{
    public record ProjectMemberDto(Guid ProjectId, Guid UserId, RoleDto RoleDto);
}
