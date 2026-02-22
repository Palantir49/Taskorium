using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.WorkspaceMembers.AddUser;

public record AddProjectMemberCommand(Guid ProjectId, Guid UserId, Roles RoleDto) : ICommand<AddProjectMemberResult>;
