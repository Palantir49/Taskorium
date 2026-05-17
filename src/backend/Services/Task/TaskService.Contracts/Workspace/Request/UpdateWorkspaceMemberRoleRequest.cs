using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Workspace.Request;

public record UpdateWorkspaceMemberRoleRequest(WorkspaceRolesDto NewRole);
