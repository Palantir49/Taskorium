using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Commands.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectCreateHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<ProjectCreateCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(ProjectCreateCommand request, CancellationToken cancellationToken = default)
    {
        _ = await context.Workspaces.FindAsync([request.WorkspaceId], cancellationToken) ??
            throw new KeyNotFoundException($"Рабочая область с id: {request.WorkspaceId} не найдена");

        var project = Project.Create(
            name: request.Name,
            description: request.Description,
            abbreviation: request.Abbreviation,
            workspaceId: request.WorkspaceId
        );

        IssueStatus initStatus = IssueStatus.Create(
            name: "Новая",
            numberType: (int)IssueStatusType.Initial,
            position: 0,
            projectId: project.Id
            );

        IssueStatus processStatus = IssueStatus.Create(
            name: "В работе",
            numberType: (int)IssueStatusType.Process,
            position: 1,
            projectId: project.Id
            );

        IssueStatus successStatus = IssueStatus.Create(
            name: "Выполнено",
            numberType: (int)IssueStatusType.Success,
            position: 2,
            projectId: project.Id
            );

        IssueStatus rejectedStatus = IssueStatus.Create(
            name: "Отменено",
            numberType: (int)IssueStatusType.Rejected,
            position: 3,
            projectId: project.Id
            );

        await context.Projects.AddAsync(project, cancellationToken);
        await context.IssueStatus.AddAsync(initStatus, cancellationToken);
        await context.IssueStatus.AddAsync(processStatus, cancellationToken);
        await context.IssueStatus.AddAsync(successStatus, cancellationToken);
        await context.IssueStatus.AddAsync(rejectedStatus, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var cacheKey = $"projects_by_workspace_{project.WorkspaceId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return project.ToResponse();
    }
}
