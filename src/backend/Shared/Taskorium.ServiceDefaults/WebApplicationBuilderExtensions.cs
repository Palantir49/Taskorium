using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;

namespace Taskorium.ServiceDefaults;

public static class WebApplicationBuilderExtensions
{
    extension(WebApplication app)
    {
        public void UseServiceDefaults(IConfiguration configuration)
        {
            var metricsExporter = configuration.GetValue("OpenTelemetry:UseMetricsExporter", "CONSOLE")
                .ToUpperInvariant();
            if (metricsExporter.Equals("prometheus", StringComparison.OrdinalIgnoreCase))
            {
                app.UseOpenTelemetryPrometheusScrapingEndpoint();
            }

            app.MapHealthChecks();
        }

        private void MapHealthChecks()
        {
            // Kubernetes liveness probe (только "живость")
            app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = r => r.Tags.Contains("live") });

            // Kubernetes readiness probe (готовность принимать трафик)
            app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = r => r.Tags.Contains("ready") });

            // Полная диагностика (все проверки)
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true // без фильтра
            });
        }
    }
}
