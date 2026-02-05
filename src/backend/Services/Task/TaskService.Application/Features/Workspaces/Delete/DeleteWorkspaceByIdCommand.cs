using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Delete;

public record DeleteWorkspaceByIdCommand(Guid Id) : ICommand<DeleteWorkspaceByIdResult>;
