using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Requirements;

namespace TaskService.Api.Authorization.Handlers;

/// <summary>
///     Обработчик авторизации проектов
/// </summary>
public class ProjectAccessHandler : AuthorizationHandler<IssueAccessRequirement>
{
    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IssueAccessRequirement requirement)
    {
        throw new NotImplementedException();
    }
}
