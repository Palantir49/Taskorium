using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Taskorium.ServiceDefaults;
using TaskService.Api.Extensions;
using TaskService.Api.Handlers;
using TaskService.Api.Middlewares;
using TaskService.Api.Transformers;
using TaskService.Application.Extensions;
using TaskService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.Setup(builder.Environment.EnvironmentName);
builder.Host.ValidateServices();
builder.Services.AddServiceDefaults(builder.Configuration);
builder.Services.AddScoped<RequestObservabilityMiddleware>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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

    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
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

builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddControllers();
//configure infrastructure layer
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer();
var app = builder.Build();

// Включение CORS
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Authentication = new ScalarAuthenticationOptions { PreferredSecuritySchemes = ["Bearer"] };
        var testToken = builder.Configuration["Authentication:Jwt:TestToken"];
        if (string.IsNullOrWhiteSpace(testToken))
        {
            throw new ArgumentNullException(testToken);
        }

        options.AddHttpAuthentication("Bearer",
            opts => opts.WithToken(testToken));
    });
}

app.UseExceptionHandler();
app.UseServiceDefaults(builder.Configuration);
app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<RequestObservabilityMiddleware>();
app.Run();
