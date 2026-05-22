using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Exceptions;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskService.Application.Features.Projects.Write.CreateProject;

public class CreateProjectHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<CreateProjectCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(CreateProjectCommand command, CancellationToken cancellationToken = default)
    {
        _ = await context.Workspaces.FindAsync([command.WorkspaceId], cancellationToken) ??
            throw new KeyNotFoundException($"Рабочая область с id: {command.WorkspaceId} не найдена");

        var project = Project.Create(
            name: command.Name,
            description: command.Description,
            abbreviation: command.Abbreviation,
            workspaceId: command.WorkspaceId
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

        var existUser = await context.Users.FindAsync([command.UserId], cancellationToken);
        if (existUser is null)
        {
            throw new KeyNotFoundException($"Пользователь с  id {command.UserId} не существует");
        }

        var projectMember =
            ProjectMember.Create(project.Id, command.UserId, ProjectRoles.Creator, DateTimeOffset.UtcNow);

        await context.Projects.AddAsync(project, cancellationToken);
        await context.IssueStatus.AddAsync(initStatus, cancellationToken);
        await context.IssueStatus.AddAsync(processStatus, cancellationToken);
        await context.IssueStatus.AddAsync(successStatus, cancellationToken);
        await context.IssueStatus.AddAsync(rejectedStatus, cancellationToken);
        await context.ProjectMembers.AddAsync(projectMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var projectCacheKey = $"projects_by_workspace_{project.WorkspaceId}";
        await cache.RemoveAsync(projectCacheKey, cancellationToken);

        var userCacheKey = $"user_by_keycloak_id_{existUser.KeycloakId}";
        await cache.RemoveAsync(userCacheKey, cancellationToken);

        return project.ToResponse();
    }
}

