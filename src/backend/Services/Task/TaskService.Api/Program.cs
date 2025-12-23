using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using TaskService.Application.Interfaces;
using TaskService.Application.Services;
using TaskService.Domain.IRepositories;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Repositories;

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
//TODO: явно не верно, что я сюда притянул посгри. как сделать правильно?
builder.Services.AddDbContext<TaskServiceDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"))); //.UseSnakeCaseNamingConvention()
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();
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
