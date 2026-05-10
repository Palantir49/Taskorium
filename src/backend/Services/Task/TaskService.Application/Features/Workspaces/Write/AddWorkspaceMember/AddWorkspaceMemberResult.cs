using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;

public record AddWorkspaceMemberResult(Guid WorkspaceId, Guid UserId, WorkspaceRolesDto RoleDto);
