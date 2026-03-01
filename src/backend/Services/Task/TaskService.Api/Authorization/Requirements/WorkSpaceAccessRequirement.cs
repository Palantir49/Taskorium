using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;

namespace TaskService.Api.Authorization.Requirements;

/// <summary>
///     WorkSpaceAccessRequirement
/// </summary>
/// <param name="Action"></param>
public record WorkSpaceAccessRequirement(WorkSpaceAction Action) : IAuthorizationRequirement;
