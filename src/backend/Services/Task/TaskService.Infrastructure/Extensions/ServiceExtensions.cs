using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Extensions.Cache;
using TaskService.Infrastructure.Extensions.Services.FileStorage;
using TaskService.Infrastructure.Interceptors;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureInfrastructureLayer(IConfiguration configuration)
        {
            services.AddSingleton<SoftDeleteInterceptor>();
            services.AddDbContext<TaskServiceDbContext>((sp, options) =>
                {
                    options.UseNpgsql(configuration.GetConnectionString("DevDefaultConnection"));
                    options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
                });
            services.AddCache(configuration); 
            services.ConfigureGrpcFileStorageClient(configuration);
        }
    }
}
