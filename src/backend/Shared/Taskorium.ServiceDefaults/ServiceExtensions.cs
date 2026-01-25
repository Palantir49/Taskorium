using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Taskorium.ServiceDefaults.OpenTelemetry;

namespace Taskorium.ServiceDefaults;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void AddServiceDefaults(IConfiguration configuration)
        {
            services.ConfigureHealthChecks();
            services.ConfigureSerilog(configuration);
            var useOtlp = configuration.GetValue<bool>("OpenTelemetry:UseOTLP");
            if (useOtlp)
            {
                services.AddSingleton<InstrumentationSource>();
                services.ConfigureOpenTelemetry(configuration);
            }
        }


        private void ConfigureOpenTelemetry(IConfiguration configuration)
        {
            var serviceName = configuration["OpenTelemetry:ServiceName"] ??
                              throw new Exception("Не задано имя сервиса для OTLP");
            var endpoint = configuration["OpenTelemetry:Endpoint"] ?? throw new Exception("Не задан адрес OTLP");
            var metricsExporter = configuration.GetValue("OpenTelemetry:UseMetricsExporter", "CONSOLE")
                .ToUpperInvariant();

            services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder =>
                {
                    tracerProviderBuilder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddEntityFrameworkCoreInstrumentation()
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                        .AddSource($"{serviceName}*")
                        .AddOtlpExporter(opts =>
                        {
                            opts.Endpoint = new Uri(endpoint);
                            opts.Protocol = OtlpExportProtocol.Grpc;
                        });
                }).WithMetrics(builder =>
                {
                    // Metrics

                    // Ensure the MeterProvider subscribes to any custom Meters.
                    builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                        .AddRuntimeInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation();

                    switch (metricsExporter)
                    {
                        case "PROMETHEUS":
                            builder.AddPrometheusExporter();
                            break;
                        case "OTLP":
                            builder.AddOtlpExporter(otlpOptions =>
                            {
                                otlpOptions.Endpoint = new Uri(endpoint);
                                otlpOptions.Protocol = OtlpExportProtocol.Grpc;
                            });
                            break;
                        default:
                            builder.AddConsoleExporter();
                            break;
                    }
                }).WithLogging(loggingOptions => loggingOptions
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName).AddAttributes(
                        new Dictionary<string, object>
                        {
                            ["environment"] =
                                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "development",
                            ["host.name"] = Environment.MachineName
                        }))
                    .AddOtlpExporter(opts =>
                        {
                            opts.Endpoint = new Uri(endpoint);
                            opts.Protocol = OtlpExportProtocol.Grpc;
                        }
                    ));
        }


        private void ConfigureSerilog(IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, true));
        }


        private void ConfigureHealthChecks()
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live", "ready"]);
        }
    }
}
