using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TaskService.Api.Authorization.Utils;

/// <summary>
///     Common authorization utils
/// </summary>
internal static class AuthorizationUtils
{
    private const string KeyCloakUserIdAttribute =
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

    internal static Guid? GetIdFromRoute(IHttpContextAccessor httpContextAccessor, string paramName = "id")
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return null;
        }

        var routeData = httpContext.GetRouteData();
        if (routeData.Values.TryGetValue(paramName, out var idValue) &&
            Guid.TryParse(idValue?.ToString(), out var id))
        {
            return id;
        }

        return null;
    }

    internal static Guid? GetKeycloakUserId(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return null;
        }

        return Guid.Parse(httpContext.User.FindFirst(KeyCloakUserIdAttribute)?.Value ?? string.Empty);
    }
}
