using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Commands.Workspaces.Create;

public record CreateWorkspaceCommand(string Name,Guid OwnerId) : ICommand<CreateWorkspaceResult>;
