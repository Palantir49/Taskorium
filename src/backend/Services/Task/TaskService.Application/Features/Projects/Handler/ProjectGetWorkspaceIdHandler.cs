using Microsoft.EntityFrameworkCore;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectGetWorkspaceIdHandler(TaskServiceDbContext context) : IRequestHandler<ProjectGetByWorkspaceIdQuery, IEnumerable<ProjectResponse>>
{
    public async Task<IEnumerable<ProjectResponse>> Handle(ProjectGetByWorkspaceIdQuery request, CancellationToken cancellationToken = default)
    {
        List<Project> projects = await context.Projects.Where(x => x.WorkspaceId == request.id).ToListAsync(cancellationToken);

        return projects.Select(x => x.ToResponse());
    }
}
