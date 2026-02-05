using TaskService.Application.Commands.Workspaces.Create;
using TaskService.Application.Mediator;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Commands.Workspaces.Get;

public class GetWorkspaceHandler(TaskServiceDbContext context) : IRequestHandler<GetWorkspaceByIdQuery, GetWorkspacebyIdResult>
{
    public async Task<GetWorkspacebyIdResult> Handle(GetWorkspaceByIdQuery query, CancellationToken cancellationToken)
    {
        var workspace = await context.Workspaces.FindAsync(query.id, cancellationToken);
        if (workspace == null)
        {
            throw new NullReferenceException($"Рабочая область с id: {query.id} не найдена");
        }
        return new GetWorkspacebyIdResult(
            id: workspace.Id,
            name: workspace.Name.ToString());
    }


}

