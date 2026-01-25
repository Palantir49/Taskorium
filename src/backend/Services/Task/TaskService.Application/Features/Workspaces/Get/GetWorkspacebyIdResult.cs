using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Commands.Workspaces.Get;

public record GetWorkspacebyIdResult(Guid id, string name);

