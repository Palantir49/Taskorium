using FileStorageService.Api.Interceptors;
using FileStorageService.Api.Services;
using FileStorageService.Application;
using FileStorageService.Application.Interfaces;
using FileStorageService.Infrastructure.Data;
using FileStorageService.Infrastructure.MinIO;
using FileStorageService.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Minio;
using Scalar.AspNetCore;
using Taskorium.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Setup(builder.Environment.EnvironmentName);
builder.Host.ValidateServices();
builder.Services.AddServiceDefaults(builder.Configuration);
// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpc(o =>
    o.Interceptors.Add<ExceptionInterceptor>());
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "1.0.0-alpha.1";
        document.Info.Title = "FileStorage Service API";
        document.Info.Description = "API для работы с файлами";
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

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// MinIO
builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var config = builder.Configuration.GetSection("MinIO");
    return new MinioClient()
        .WithEndpoint(config["Endpoint"])
        .WithCredentials(config["AccessKey"], config["SecretKey"])
        .WithSSL(config.GetValue<bool>("UseSSL"))
        .Build();
});

//// Redis Cache
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("Redis");
//});

// Application Services
builder.Services.AddScoped<IFileStorageService, FileStorageService.Application.Services.FileStorageService>();
//builder.Services.AddScoped<IFileMetadataService, FileMetadataService>();
builder.Services.AddScoped<IMinioService, MinioService>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//// Health Checks
//builder.Services.AddHealthChecks()
//    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!)
//    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseServiceDefaults(builder.Configuration);
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();
app.MapGrpcService<FileService>();

app.Run();
