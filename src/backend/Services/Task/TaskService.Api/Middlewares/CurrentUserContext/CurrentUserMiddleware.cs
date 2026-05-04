using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Users.Read.Query;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;

namespace TaskService.Api.Middlewares.CurrentUserContext
{
    /// <summary>
    /// Middleware for getting the current user
    /// </summary>
    public class CurrentUserMiddleware : IMiddleware
    {
        private readonly ICurrentUserContext _userContext;
        private readonly IDispatcher _dispatcher;
        private readonly ILogger<CurrentUserMiddleware> _logger;

        private const string KeyCloakUserIdAttribute =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="userContext"></param>
        /// <param name="logger"></param>
        public CurrentUserMiddleware(
            IDispatcher dispatcher,
        ICurrentUserContext userContext,
            ILogger<CurrentUserMiddleware> logger)
        {
            _dispatcher = dispatcher;
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Invoke middleware
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var claimValue = context.User
            .FindFirst(KeyCloakUserIdAttribute)?.Value;

            if (string.IsNullOrWhiteSpace(claimValue) || !Guid.TryParse(claimValue, out var keycloakId))
            {
                await next(context);
                return;
            }

            try
            {
                var query = new GetUserByKeycloakIdQuery(keycloakId);
                var user = await _dispatcher.SendAsync(query, context.RequestAborted);

                if (user is not null)
                {
                    _userContext.Initialize(user);
                }
                else
                {
                    _logger.LogWarning($"User with KeycloakId {keycloakId} not found in local DB.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load user context for KeycloakId {keycloakId}.");
            }

            await next(context);
        }
    }
}
