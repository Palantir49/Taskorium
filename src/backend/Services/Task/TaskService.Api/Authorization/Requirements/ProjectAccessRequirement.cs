using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;

namespace TaskService.Api.Authorization.Requirements;

/// <summary>
/// </summary>
/// <param name="Action"></param>
public record ProjectAccessRequirement(ProjectAction Action) : IAuthorizationRequirement;
