using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Contracts.User.Responses
{
    public record class UsersWorkspaceResponse(Guid Id,
                                              WorkspaceRolesDto Role,
                                              string Name,
                                              DateTimeOffset CreatedDate);

}
