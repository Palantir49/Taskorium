using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Read.Result;

public record GetWorkspacebyIdResult(Guid id, string name);

