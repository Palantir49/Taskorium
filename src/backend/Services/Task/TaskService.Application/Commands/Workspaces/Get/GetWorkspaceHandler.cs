using TaskService.Application.Commands.Workspaces.Create;
using TaskService.Application.Mediator;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Workspaces.Get;

public class GetWorkspaceHandler : IQueryHandler<GetWorkspaceByIdQuery, GetWorkspacebyIdResult>
{
    private readonly IRepositoryWrapper _wrapper;

    public GetWorkspaceHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<GetWorkspacebyIdResult> Handle(GetWorkspaceByIdQuery query, CancellationToken cancellationToken)
    {
        var workspace = await _wrapper.Workspaces.GetByIdAsync(query.id, cancellationToken);
        if (workspace == null)
        {
            throw new NullReferenceException($"Рабочая область с id: {query.id} не найдена");
        }
        return new GetWorkspacebyIdResult(
            id: workspace.Id,
            name: workspace.Name.ToString());
    }


}

