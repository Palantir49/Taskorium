using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;

namespace TaskService.Application.Features.WorkspaceMembers.AddUser;

public record AddProjectMemberCommand(Guid ProjectId, Guid UserId, RolesDto RoleDto) : ICommand<AddProjectMemberResult>;
