using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectGetWorkspaceIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<ProjectGetByWorkspaceIdQuery, IEnumerable<ProjectResponse>>
{
    public async Task<IEnumerable<ProjectResponse>> Handle(ProjectGetByWorkspaceIdQuery request, CancellationToken cancellationToken = default)
    {
        List<Project> projects = await wrapper.Projects.GetByWorkspaceIdAsync(request.id);

        return projects.Select(x => x.ToResponse());
    }
}
