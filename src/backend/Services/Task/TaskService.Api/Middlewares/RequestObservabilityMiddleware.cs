using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TaskService.Api.Middlewares;

/// <summary>
///     Observability middleware
/// </summary>
public class RequestObservabilityMiddleware : IMiddleware
{
    private readonly ILogger<RequestObservabilityMiddleware> _logger;

    /// <summary>
    ///     Ctr
    /// </summary>
    /// <param name="logger"></param>
    public RequestObservabilityMiddleware(ILogger<RequestObservabilityMiddleware> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Invoke middleware
    /// </summary>
    /// <param name="context">Http-context</param>
    /// <param name="next">Executing middleware</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var startTime = Stopwatch.GetTimestamp();
        _logger.LogInformation("Начало выполнения запроса {Method}, {Path}", context.Request.Method,
            context.Request.Path);
        await next(context);
        var diff = Stopwatch.GetElapsedTime(startTime);
        _logger.LogInformation("Конец выполнения запроса {Method}, {Path}. Выполнен за {Time} мс.",
            context.Request.Method,
            context.Request.Path, diff.Milliseconds);
    }
}
