using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Project.Requests
{
    public record AddProjectMemberRequest(Guid UserId, RolesDto RoleDto);
}
