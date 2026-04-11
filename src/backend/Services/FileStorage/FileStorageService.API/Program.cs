using FileStorageService.Api.Interceptors;
using FileStorageService.Api.Services;
using FileStorageService.Application.Interfaces;
using FileStorageService.Infrastructure.MinIO;
using FluentValidation;
using FluentValidation.AspNetCore;
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
    options.AddDocumentTransformer((document, _, _) =>
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

// MinIO
builder.Services.AddSingleton<IMinioClient>(_ =>
{
    var config = builder.Configuration.GetSection("MinIO");
    return new MinioClient()
        .WithEndpoint(config["Endpoint"])
        .WithCredentials(config["AccessKey"], config["SecretKey"])
        .WithSSL(config.GetValue<bool>("UseSSL"))
        .Build();
});


// Application Services
builder.Services.AddScoped<IFileStorageService, FileStorageService.Application.Services.FileStorageService>();
builder.Services.AddScoped<IMinioService, MinioService>();

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", config =>
    {
        config.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseServiceDefaults(builder.Configuration);
app.UseCors("AllowAll");
app.MapControllers();
app.MapGrpcService<FileService>();

app.Run();
