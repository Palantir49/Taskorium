using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Repositories;

namespace TaskService.Infrastructure.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureInfrastructureLayer(IConfiguration configuration)
        {
            services.AddDbContext<TaskServiceDbContext>(options => options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IIssueRepository, IssueRepository>();
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        }
    }
}
