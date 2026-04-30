using TaskService.Application.Features.Workspaces.Read.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Read.Query;

public record GetWorkspacePageQuery(int Skip, int Take) : IQuery<GetWorkspacePageResult>;
