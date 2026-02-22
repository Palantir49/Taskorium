using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Get;

public record GetWorkspacePageQuery(int skip, int take) : IQuery<GetWorkspacePageResult>;
