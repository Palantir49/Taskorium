using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;
using TaskService.Api.Authorization.Requirements;
using TaskService.Api.Authorization.Utils;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Projects.Read.Query;
using TaskService.Application.Interfaces;
using TaskService.Application.Features.Users.Read.Query;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;

namespace TaskService.Api.Authorization.Handlers;

/// <summary>
///     Обработчик авторизации для задач
/// </summary>
public class IssueAccessHandler(
    IHttpContextAccessor httpContextAccessor,
    IDispatcher dispatcher,
    ILogger<IssueAccessHandler> logger,
    ICurrentUserContext userContext)
    : AuthorizationHandler<IssueAccessRequirement>
{
    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IssueAccessRequirement requirement)
    {
        logger.LogInformation("Начало процесса авторизация для совершения действия: {Action} над задачей",
            requirement.Action);

        if (requirement.Action == IssueAction.Create)
        {
            context.Succeed(requirement);
        }
        var issueId = AuthorizationUtils.GetIdFromRoute(httpContextAccessor, "issueId");
        if (issueId is null)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над задачей произошла ошибка: не удалось получить идентификатор задачи из запроса",
                requirement.Action);
            context.Fail();
            return;
        }

        //get task
        var issueQuery = new IssueGetByIdQuery(issueId);
        var issue = await dispatcher.SendAsync(issueQuery);


        //get project
        var projectQuery = new GetProjectByIdQuery(issue.ProjectId);
        var project = await dispatcher.SendAsync(projectQuery);


        if (!userContext.IsInitialized)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над задачей {issueId} произошла ошибка: контекст текущего пользователя не инициализирован",
                requirement.Action, issueId);
            context.Fail();
            return;
        }

        var user = userContext.User;

        var wsMemberShip = user.WorkSpaceMembers?.FirstOrDefault(x => x.WorkspaceId == project.WorkspaceId);

        switch (wsMemberShip?.Role) //TODO enum
        {
            case RolesDto.Creator:

            case RolesDto.Admin:
                context.Succeed(requirement);
                return;

            case RolesDto.Member:
                if (requirement.Action is IssueAction.View or IssueAction.Update)
                {
                    context.Succeed(requirement);
                    return;
                }

                break;
            case RolesDto.Viewer:
                if (requirement.Action == IssueAction.View)
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

            case RolesDto.Admin:
                context.Succeed(requirement);
                return;

            case RolesDto.Member: //TODO add user assigned task issue
                if (requirement.Action is IssueAction.View or IssueAction.Update)
                {
                    context.Succeed(requirement);
                }

                break;
            case RolesDto.Viewer:
                if (requirement.Action == IssueAction.View)
                {
                    context.Succeed(requirement);
                }

                break;
        }

        logger.LogInformation(
            "В процессе авторизации для совершения действия {Action} над задачей {issueId} доступ не предоставлен: отсутствуют необходимые роли участника workspace/project",
            requirement.Action, issueId);
        context.Fail();
    }
}
