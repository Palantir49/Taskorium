using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureInfrastructureLayer(IConfiguration configuration)
        {
            services.AddDbContext<TaskServiceDbContext>(options => options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
