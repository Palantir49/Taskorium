using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Read.GetWorkspaceById;

public record GetWorkspaceByIdQuery(Guid? Id) : IQuery<GetWorkspacebyIdResult>;
