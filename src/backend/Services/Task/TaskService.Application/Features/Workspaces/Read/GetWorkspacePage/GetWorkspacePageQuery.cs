using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Read.GetWorkspacePage;

public record GetWorkspacePageQuery(int Skip = 0, int Take = 50, Guid? UserId = null) : IQuery<GetWorkspacePageResult>;
