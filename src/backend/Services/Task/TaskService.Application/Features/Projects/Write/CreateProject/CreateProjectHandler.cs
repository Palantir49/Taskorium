using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Users.Write.CreateUser;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Write.CreateProject;

public class CreateProjectHandler(TaskServiceDbContext context, HybridCache cache, IValidator<CreateProjectCommand> validator)
    : IRequestHandler<CreateProjectCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(CreateProjectCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        _ = await context.Workspaces.FindAsync([command.WorkspaceId], cancellationToken) ??
            throw new KeyNotFoundException($"Рабочая область с id: {command.WorkspaceId} не найдена");

        var project = Project.Create(
            command.Name,
            command.Description,
            command.Abbreviation,
            command.WorkspaceId
        );

        IssueStatus initStatus = IssueStatus.Create(
            name: "Новая",
            type: IssueStatusType.Initial,
            position: 0,
            projectId: project.Id,
            color: "#6B7280"
        );

        IssueStatus processStatus = IssueStatus.Create(
            name: "В работе",
            type: IssueStatusType.Process,
            position: 1,
            projectId: project.Id,
            color: "#3B82F6"
        );

        IssueStatus successStatus = IssueStatus.Create(
            name: "Выполнено",
            type: IssueStatusType.Success,
            position: 2,
            projectId: project.Id,
            color: "#10B981"
        );

        IssueStatus rejectedStatus = IssueStatus.Create(
            name: "Отменено",
            type: IssueStatusType.Rejected,
            position: 3,
            projectId: project.Id,
            color: "#DC2626"
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

        return project.ToResponse(command.UserId);
    }
}
