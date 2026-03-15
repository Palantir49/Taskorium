using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;
using TaskService.Api.Authorization.Requirements;
using TaskService.Api.Authorization.Utils;
using TaskService.Application.Features.Users.Get;
using TaskService.Application.Mediator;

namespace TaskService.Api.Authorization.Handlers;

/// <summary>
///     Обработчик авторизации для рабоичих
/// </summary>
public class WorkSpaceAccessHandler(
    IHttpContextAccessor httpContextAccessor,
    IDispatcher dispatcher,
    ILogger<ProjectAccessHandler> logger)
    : AuthorizationHandler<WorkSpaceAccessRequirement>
{
    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        WorkSpaceAccessRequirement requirement)
    {
        logger.LogInformation("Начало процесса авторизация для совершения действия: {Action} над рабочей область",
            requirement.Action);
        var workspaceId = AuthorizationUtils.GetIdFromRoute(httpContextAccessor);
        if (workspaceId is null)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над рабочей областью произошла ошибка: не удалось получить идентификатор задачи из запроса",
                requirement.Action);
            context.Fail();
            return;
        }


        var userKeyCloakId = AuthorizationUtils.GetKeycloakUserId(httpContextAccessor);
        if (userKeyCloakId is null)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над рабочей областью {workspaceId} произошла ошибка: не удалось получить keycloakId пользователя из запроса",
                requirement.Action, workspaceId);
            context.Fail();
            return;
        }

        //get user
        var user = await dispatcher.SendAsync(new GetUserByKeycloakIdQuery(userKeyCloakId));

        var wsMemberShip = user.WorkSpaceMembers?.FirstOrDefault(x => x.WorkspaceId == workspaceId);

        switch (wsMemberShip?.RoleDto.roleName) //TODO enum
        {
            case "Creator":

            case "Admin": //TODO Set (unset) admin
                context.Succeed(requirement);
                return;

            case "Member":
                if (requirement.Action is WorkSpaceAction.View or WorkSpaceAction.Update)
                {
                    context.Succeed(requirement);
                }

                break;
            case "Viewer":
                if (requirement.Action == WorkSpaceAction.View)
                {
                    context.Succeed(requirement);
                }

                break;
        }
    }
}
