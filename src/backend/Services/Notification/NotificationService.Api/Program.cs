using Microsoft.OpenApi;
using NotificationService.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Taskorium.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Setup(builder.Environment.EnvironmentName);
builder.Host.ValidateServices();
builder.Services.AddServiceDefaults(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "1.0.0-alpha.1";
        document.Info.Title = "Сервис уведомлений";
        document.Info.Description = "Сервис уведомлений";
        document.Info.Contact = new OpenApiContact
        {
            Name = "https://github.com/Palantir49",
            Email = "Vadim Ryzhenkov",
            Url = new Uri("https://github.com/Palantir49")
        };

        document.Info.License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };
        return Task.CompletedTask;
    });
});
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
var app = builder.Build();
app.UseServiceDefaults(builder.Configuration);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Run();
