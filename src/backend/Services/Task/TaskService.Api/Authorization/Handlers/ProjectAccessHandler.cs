using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;
using TaskService.Api.Authorization.Requirements;
using TaskService.Api.Authorization.Utils;
using TaskService.Application.Features.Projects.Read.GetProjectById;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;

namespace TaskService.Api.Authorization.Handlers;

/// <summary>
///     Обработчик авторизации для проектов
/// </summary>
public class ProjectAccessHandler(
    IHttpContextAccessor httpContextAccessor,
    IDispatcher dispatcher,
    ILogger<ProjectAccessHandler> logger,
    ICurrentUserContext userContext)
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

        var projectId = AuthorizationUtils.GetIdFromRoute(httpContextAccessor, "projectId");
        if (projectId is null)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над проектом произошла ошибка: не удалось получить идентификатор проекта из запроса",
                requirement.Action);
            context.Fail();
            return;
        }


        //get project
        var projectQuery = new GetProjectByIdQuery(projectId);
        var project = await dispatcher.SendAsync(projectQuery);


        if (!userContext.IsInitialized)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над проектом {projectId} произошла ошибка: контекст текущего пользователя не инициализирован",
                requirement.Action, projectId);
            context.Fail();
            return;
        }

        var user = userContext.User;

        var wsMemberShip = user.WorkSpaceMembers?.FirstOrDefault(x => x.WorkspaceId == project.WorkspaceId);

        switch (wsMemberShip?.Role) //TODO enum
        {
            case WorkspaceRolesDto.Creator:

            case WorkspaceRolesDto.Admin:
                context.Succeed(requirement);
                break;

            case WorkspaceRolesDto.Viewer:
                if (requirement.Action == ProjectAction.View)
                {
                    context.Succeed(requirement);
                    break;
                }

                break;
        }

        var projectMemberShip = user.ProjectMembers?.FirstOrDefault(x => x.ProjectId == project.Id);
        switch (projectMemberShip?.Role) //TODO enum
        {
            case ProjectRolesDto.Creator:

            case ProjectRolesDto.Admin: //TODO Set and unset admin in project
                context.Succeed(requirement);
                return;

            case ProjectRolesDto.Member: //TODO add user assigned task issue
                if (requirement.Action is ProjectAction.View or ProjectAction.Update)
                {
                    context.Succeed(requirement);
                }

                break;
            case ProjectRolesDto.Viewer:
                if (requirement.Action == ProjectAction.View)
                {
                    context.Succeed(requirement);
                }

                break;
        }

        logger.LogInformation(
            "В процессе авторизации для совершения действия {Action} над проектом {projectId} доступ не предоставлен: отсутствуют необходимые роли участника workspace/project",
            requirement.Action, projectId);
        context.Fail();
    }
}
