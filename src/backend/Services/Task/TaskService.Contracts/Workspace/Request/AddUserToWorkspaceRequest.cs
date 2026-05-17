using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Workspace.Request;

public record AddUserToWorkspaceRequest(Guid UserId, WorkspaceRolesDto Role);

