using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Extensions.Cache;
using TaskService.Infrastructure.Extensions.Services.FileStorage;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureInfrastructureLayer(IConfiguration configuration)
        {
            services.AddDbContext<TaskServiceDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                );
            services.AddCache(configuration);
            services.ConfigureGrpcFileStorageClient(configuration);
        }
    }
}
