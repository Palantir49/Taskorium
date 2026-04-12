using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taskorium.FileStorage.V1;

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
