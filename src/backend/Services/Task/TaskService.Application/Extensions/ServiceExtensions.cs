using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Interfaces;
using TaskService.Application.Services;
using TaskService.Domain.IRepositories;

namespace TaskService.Application.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureApplicationLayer()
        {
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();
        }
    }
}
