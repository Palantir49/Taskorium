using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;

namespace TaskService.Application.Features.Projects.Write.AddProjectMember;

public record AddProjectMemberCommand(Guid ProjectId, Guid UserId, ProjectRolesDto RoleDto) : ICommand<AddProjectMemberResult>;
