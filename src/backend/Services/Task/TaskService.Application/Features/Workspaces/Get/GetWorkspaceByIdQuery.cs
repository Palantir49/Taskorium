using TaskService.Application.Commands.Workspaces.Get;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Get;

public record GetWorkspaceByIdQuery(Guid? Id) : IQuery<GetWorkspacebyIdResult>;
