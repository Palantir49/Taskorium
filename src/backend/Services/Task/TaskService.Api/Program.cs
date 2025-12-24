using Microsoft.OpenApi;
using Scalar.AspNetCore;
using TaskService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
//configure infrastructure layer
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
