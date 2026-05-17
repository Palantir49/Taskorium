using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Read.GetWorkspaceById;

public record GetWorkspacebyIdResult(Guid id, string name);

