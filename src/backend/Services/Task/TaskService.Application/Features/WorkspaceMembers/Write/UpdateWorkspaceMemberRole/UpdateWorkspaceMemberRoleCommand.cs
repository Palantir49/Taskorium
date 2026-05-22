using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.WorkspaceMembers.Write.Command;

public record UpdateWorkspaceMemberRoleCommand(Guid WorkspaceId, Guid UserId, WorkspaceRolesDto NewRole) : ICommand<AddWorkspaceMemberResult>;
