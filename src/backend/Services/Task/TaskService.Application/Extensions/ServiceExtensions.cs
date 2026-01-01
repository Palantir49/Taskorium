using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Handlers.Issues.Handler;
using TaskService.Application.Handlers.Projects.Handler;
using TaskService.Application.Interfaces;
using TaskService.Application.Services;

namespace TaskService.Application.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureApplicationLayer()
        {
            services.AddScoped<IWorkspaceService, WorkspaceService>();

            services.AddScoped<CreateIssueHandler, CreateIssueHandler>();
            services.AddScoped<CreateProjectHandler, CreateProjectHandler>();
        }
    }
}
