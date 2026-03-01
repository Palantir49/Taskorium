using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;
using TaskService.Api.Authorization.Requirements;
using TaskService.Api.Authorization.Utils;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Features.Users.Get;
using TaskService.Application.Mediator;

namespace TaskService.Api.Authorization.Handlers;

/// <summary>
///     Обработчик авторизации для задач
/// </summary>
public class IssueAccessHandler(
    IHttpContextAccessor httpContextAccessor,
    IDispatcher dispatcher,
    ILogger<IssueAccessHandler> logger)
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
        var issueId = AuthorizationUtils.GetIdFromRoute(httpContextAccessor);
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
        var projectQuery = new ProjectGetByIdQuery(issue.ProjectId);
        var project = await dispatcher.SendAsync(projectQuery);


        var userKeyCloakId = AuthorizationUtils.GetKeycloakUserId(httpContextAccessor);
        if (userKeyCloakId is null)
        {
            logger.LogInformation(
                "В процессе авторизации для совершения действия {Action} над задачей {issueId} произошла ошибка: не удалось получить keycloakId пользователя из запроса",
                requirement.Action, issueId);
            context.Fail();
            return;
        }

        //get user
        var user = await dispatcher.SendAsync(new GetUserByKeycloakIdQuery(userKeyCloakId));

        var wsMemberShip = user.WorkSpaceMembers?.FirstOrDefault(x => x.WorkspaceId == project.WorkspaceId);

        switch (wsMemberShip?.RoleDto.roleName) //TODO enum
        {
            case "Creator":

            case "Admin":
                context.Succeed(requirement);
                return;

            case "Member":
                if (requirement.Action is IssueAction.View or IssueAction.Update)
                {
                    context.Succeed(requirement);
                    return;
                }

                break;
            case "Viewer":
                if (requirement.Action == IssueAction.View)
                {
                    context.Succeed(requirement);
                    return;
                }

                break;
        }

        var projectMemberShip = user.ProjectMembers?.FirstOrDefault(x => x.ProjectId == project.Id);
        switch (projectMemberShip?.RoleDto.roleName) //TODO enum
        {
            case "Creator":

            case "Admin":
                context.Succeed(requirement);
                return;

            case "Member": //TODO add user assigned task issue
                if (requirement.Action is IssueAction.View or IssueAction.Update)
                {
                    context.Succeed(requirement);
                }

                break;
            case "Viewer":
                if (requirement.Action == IssueAction.View)
                {
                    context.Succeed(requirement);
                }

                break;
        }
    }
}
