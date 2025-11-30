# Требования к разработке .NET сервисов

## 1. Общие требования

### 1.1 Версии .NET

- **Целевая платформа**: .NET 10.0
- **Минимальная версия SDK**: .NET 10.0.100
- **Конфигурация**: Указывается в `Directory.Build.props`

## 2. Observability требования

### 2.1 Использование Opentelemetry для логов, трэйсов и метрик

- пример

```csharp
  builder.Services.AddOpenTelemetry()
  .ConfigureResource(resource => resource
  .AddService(serviceName: builder.Environment.ApplicationName))
  .WithTracing(tracing => tracing
  .AddAspNetCoreInstrumentation()
  .AddHttpClientInstrumentation()
  .AddEntityFrameworkCoreInstrumentation()
  .AddOtlpExporter(options =>
  {
  options.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
  }))
  .WithMetrics(metrics => metrics
  .AddAspNetCoreInstrumentation()
  .AddHttpClientInstrumentation()
  .AddRuntimeInstrumentation()
  .AddOtlpExporter(options =>
  {
  options.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
  }));
  ```

### 2.2 Логирование

Требования к логам:

- Использование структурированного логирования;
- Контекстные данные в каждом логе
- Correlation ID для трейсинга

## 3. Health Checks требования

### 3.1 Endpoints

- Liveness probe: /liveness
- Readiness probe: /readiness

## 4. Docker требования

- наличие multistage docker файла;
- наличие файла .dockerignore 


