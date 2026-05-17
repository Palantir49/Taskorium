using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Workspace.Response;

public record DeletedWorkspaceResponse(Guid Id, string Name, DateTimeOffset DeletedAt);

