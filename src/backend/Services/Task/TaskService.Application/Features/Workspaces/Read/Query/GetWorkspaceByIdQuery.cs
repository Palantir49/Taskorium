using TaskService.Application.Features.Workspaces.Read.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Read.Query;

public record GetWorkspaceByIdQuery(Guid? Id) : IQuery<GetWorkspacebyIdResult>;
