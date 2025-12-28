using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Handlers.Issues.Handler;
using TaskService.Application.Handlers.Projects.Handler;
using TaskService.Application.Interfaces;
using TaskService.Application.Services;
using TaskService.Application.Wrapper;

namespace TaskService.Application.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureApplicationLayer()
        {
            //services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddScoped<CreateIssueHandler, CreateIssueHandler>();
            services.AddScoped<CreateProjectHandler, CreateProjectHandler>();
        }
    }
}
