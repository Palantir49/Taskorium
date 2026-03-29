using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;
using TaskService.Api.Authorization.Requirements;
using TaskService.Api.Authorization.Utils;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Features.Users.Get;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;

namespace TaskService.Api.Authorization.Handlers;

/// <summary>
///     Обработчик авторизации для проектов
/// </summary>
public class ProjectAccessHandler(
    IHttpContextAccessor httpContextAccessor,
    IDispatcher dispatcher,
    ILogger<ProjectAccessHandler> logger)
    : AuthorizationHandler<ProjectAccessRequirement>
{
    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ProjectAccessRequirement requirement)
    {
        logger.LogInformation("Начало процесса авторизация для совершения действия: {Action} над проектом",
            requirement.Action);
        var projectId = AuthorizationUtils.GetIdFromRoute(httpContextAccessor);
        if (projectId is null)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над проектом произошла ошибка: не удалось получить идентификатор проекта из запроса",
                requirement.Action);
            context.Fail();
            return;
        }


        //get project
        var projectQuery = new ProjectGetByIdQuery(projectId);
        var project = await dispatcher.SendAsync(projectQuery);


        var userKeyCloakId = AuthorizationUtils.GetKeycloakUserId(httpContextAccessor);
        if (userKeyCloakId is null)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над проектом {projectId} произошла ошибка: не удалось получить keycloakId пользователя из запроса",
                requirement.Action, projectId);
            context.Fail();
            return;
        }

        //get user
        var user = await dispatcher.SendAsync(new GetUserByKeycloakIdQuery(userKeyCloakId));

        var wsMemberShip = user.WorkSpaceMembers?.FirstOrDefault(x => x.WorkspaceId == project.WorkspaceId);

        switch (wsMemberShip?.Role) //TODO enum
        {
            case RolesDto.Creator:

            case RolesDto.Admin:
                context.Succeed(requirement);
                return;

            case RolesDto.Member:
                if (requirement.Action is ProjectAction.View or ProjectAction.Update)
                {
                    context.Succeed(requirement);
                    return;
                }

                break;
            case RolesDto.Viewer:
                if (requirement.Action == ProjectAction.View)
                {
                    context.Succeed(requirement);
                    return;
                }

                break;
        }

        var projectMemberShip = user.ProjectMembers?.FirstOrDefault(x => x.ProjectId == project.Id);
        switch (projectMemberShip?.Role) //TODO enum
        {
            case RolesDto.Creator:

            case RolesDto.Admin: //TODO Set and unset admin in project
                context.Succeed(requirement);
                return;

            case RolesDto.Member: //TODO add user assigned task issue
                if (requirement.Action is ProjectAction.View or ProjectAction.Update)
                {
                    context.Succeed(requirement);
                }

                break;
            case RolesDto.Viewer:
                if (requirement.Action == ProjectAction.View)
                {
                    context.Succeed(requirement);
                }

                break;
        }
    }
}
