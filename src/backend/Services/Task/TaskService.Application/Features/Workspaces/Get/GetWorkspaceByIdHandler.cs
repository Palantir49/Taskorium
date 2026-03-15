using TaskService.Application.Features.Workspaces.Get;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Commands.Workspaces.Get;

public class GetWorkspaceHandler(TaskServiceDbContext context)
    : IRequestHandler<GetWorkspaceByIdQuery, GetWorkspacebyIdResult>
{
    public async Task<GetWorkspacebyIdResult> Handle(GetWorkspaceByIdQuery query, CancellationToken cancellationToken)
    {
        var workspace = await context.Workspaces.FindAsync(query.Id, cancellationToken);
        if (workspace == null)
        {
            throw new NullReferenceException($"Рабочая область с id: {query.Id} не найдена");
        }

        return new GetWorkspacebyIdResult(
            workspace.Id,
            workspace.Name.ToString());
    }
}
