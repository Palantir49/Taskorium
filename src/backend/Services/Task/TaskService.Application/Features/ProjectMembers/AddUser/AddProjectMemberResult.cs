using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.WorkspaceMembers.AddUser;

public record AddProjectMemberResult(Guid ProjectId, Guid UserId, ProjectRolesDto RoleDto);
