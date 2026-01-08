using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Commands.Workspaces.Create;

public record class CreateWorkspaceCommand(string Name, Guid? ownerId = null) : ICommand<CreateWorkspaceResult>;
