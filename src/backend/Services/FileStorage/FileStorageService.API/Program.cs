using FileStorageService.Application;
using FileStorageService.Domain;
using FileStorageService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Minio;
using Serilog;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// TODO: Swagger or Scalar?
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "File Storage API", Version = "v1" });
//});

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
builder.Services.AddScoped<IFileStorageService, FileStorageService.Application.FileStorageService>();
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

//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
