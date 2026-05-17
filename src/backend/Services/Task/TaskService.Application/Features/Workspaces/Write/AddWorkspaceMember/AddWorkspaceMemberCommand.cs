using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;

public record AddWorkspaceMemberCommand(Guid WorkspaceId, Guid UserId, WorkspaceRolesDto Role) : ICommand<AddWorkspaceMemberResult>;
