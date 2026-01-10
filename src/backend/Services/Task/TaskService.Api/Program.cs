using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Taskorium.ServiceDefaults;
using TaskService.Api.Middlewares;
using TaskService.Application.Extensions;
using TaskService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables()
    .Build();

builder.Host.ValidateServices();
builder.Services.AddServiceDefaults(builder.Configuration);
builder.Services.AddScoped<RequestObservabilityMiddleware>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "1.0.0-alpha.1";
        document.Info.Title = "Task Service API";
        document.Info.Description = "API для управления проектами и задачами";
        document.Info.Contact = new OpenApiContact
        {
            Name = "https://github.com/Palantir49",
            Email = "Vadim Ryzhenkov",
            Url = new Uri("https://github.com/Palantir49")
        };

        document.Info.License = new OpenApiLicense
        {
            Name = "MIT License", Url = new Uri("https://opensource.org/licenses/MIT")
        };
        return Task.CompletedTask;
    });
});

// Политику CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
//configure infrastructure layer
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer();
var app = builder.Build();

// Включение CORS
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseServiceDefaults(builder.Configuration);
app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<RequestObservabilityMiddleware>();
app.Run();
