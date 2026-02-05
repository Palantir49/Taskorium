using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Delete;

public record DeleteWorkspaceByIdResult(Guid Id, string Name);

