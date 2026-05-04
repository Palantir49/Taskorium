using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;
using TaskService.Api.Authorization.Requirements;
using TaskService.Api.Authorization.Utils;
using TaskService.Application.Features.Users.Read.Query;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;

namespace TaskService.Api.Authorization.Handlers;

/// <summary>
///     Обработчик авторизации для рабочей области
/// </summary>
public class WorkSpaceAccessHandler(
    IHttpContextAccessor httpContextAccessor,
    ILogger<WorkSpaceAccessHandler> logger,
    ICurrentUserContext userContext)
    : AuthorizationHandler<WorkSpaceAccessRequirement>
{
    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        WorkSpaceAccessRequirement requirement)
    {
        logger.LogInformation("Начало процесса авторизация для совершения действия: {Action} над рабочей областью",
            requirement.Action);
        if (requirement.Action == WorkSpaceAction.Create)
        {
            context.Succeed(requirement);
            return;
        }
        var workspaceId = AuthorizationUtils.GetIdFromRoute(httpContextAccessor, "workspaceId");
        if (workspaceId is null)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над рабочей областью произошла ошибка: не удалось получить идентификатор рабочей области из запроса",
                requirement.Action);
            context.Fail();
            return;
        }


        if (!userContext.IsInitialized)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над рабочей областью {workspaceId} произошла ошибка: контекст текущего пользователя не инициализирован",
                requirement.Action, workspaceId);
            context.Fail();
            return;
        }

        var user = userContext.User;

        var wsMemberShip = user.WorkSpaceMembers?.FirstOrDefault(x => x.WorkspaceId == workspaceId);

        switch (wsMemberShip?.Role)
        {
            case WorkspaceRolesDto.Creator:

            case WorkspaceRolesDto.Admin: //TODO Set (unset) admin
                context.Succeed(requirement);
                return;


            case WorkspaceRolesDto.Viewer:
                if (requirement.Action == WorkSpaceAction.View)
                {
                    context.Succeed(requirement);
                }

                break;
        }

        logger.LogInformation(
            "В процессе авторизации для совершения действия {Action} над рабочей областью {workspaceId} доступ не предоставлен: отсутствуют необходимые роли участника",
            requirement.Action, workspaceId);
        context.Fail();
    }
}
