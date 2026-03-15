using Microsoft.AspNetCore.Authorization;
using TaskService.Api.Authorization.Actions;

namespace TaskService.Api.Authorization.Requirements;

/// <summary>
///     Task access requirement
/// </summary>
/// <param name="Action"></param>
public record IssueAccessRequirement(IssueAction Action) : IAuthorizationRequirement;
