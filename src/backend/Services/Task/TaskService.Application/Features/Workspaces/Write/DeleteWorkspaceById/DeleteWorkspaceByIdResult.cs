using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Write.DeleteWorkspaceById;

public record DeleteWorkspaceByIdResult(Guid Id, string Name);

