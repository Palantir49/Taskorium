using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.User.Responses;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Workspaces.Read.Result;

public record GetWorkspacePageResult(IEnumerable<WorkspaceResponse> workspaces);
