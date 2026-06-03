using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.User.Responses
{
    public record UserProjectsResponse(Guid Id,
                                       ProjectRolesDto Role,
                                       string Name,
                                       DateTimeOffset CreatedDate);

}
