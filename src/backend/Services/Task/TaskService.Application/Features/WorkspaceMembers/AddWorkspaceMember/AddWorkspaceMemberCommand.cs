using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.WorkspaceMembers.AddUser;

public record AddWorkspaceMemberCommand(Guid workspaceId, Guid userId, Roles role) : ICommand<AddWorkspaceMemberResult>;
