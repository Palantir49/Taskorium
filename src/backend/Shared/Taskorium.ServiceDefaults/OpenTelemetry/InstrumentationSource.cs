using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Taskorium.ServiceDefaults.OpenTelemetry;

public sealed class InstrumentationSource : IDisposable
{
    public InstrumentationSource(IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"] ??
                          throw new Exception("Не задано имя сервиса для OTLP");
        ActivitySource = new ActivitySource(serviceName);
    }

    public ActivitySource ActivitySource { get; }

    public void Dispose()
    {
        ActivitySource.Dispose();
    }
}
