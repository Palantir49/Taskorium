using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taskorium.FileStorage.V1;
using Taskorium.ServiceDefaults;

namespace TaskService.Infrastructure.Extensions.Services.FileStorage;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        internal void ConfigureGrpcFileStorageClient(IConfiguration configuration)
        {
            var fileStorageServiceEndpoint = configuration.GetSection("Services:FileStorageService").Value;
            if (string.IsNullOrWhiteSpace(fileStorageServiceEndpoint))
            {
                if (BuildTimeDetector.IsBuildTime)
                {
                    services.AddGrpcClient<FileStorageService.FileStorageServiceClient>(o =>
                    {
                        o.Address = new Uri("http://localhost");
                    });
                    services.AddSingleton<Infrastructure.Services.FileStorageService>();
                    return;
                }

                throw new Exception("Не задан адрес файлового сервиса");
            }

            services.AddGrpcClient<FileStorageService.FileStorageServiceClient>(o =>
            {
                o.Address = new Uri(fileStorageServiceEndpoint);
            });

            services.AddSingleton<Infrastructure.Services.FileStorageService>();
        }
    }
}
