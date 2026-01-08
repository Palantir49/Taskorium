using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Commands.Issues.Handler;
using TaskService.Application.Commands.Projects.Handler;
using TaskService.Application.Commands.Workspaces.Create;
using TaskService.Application.Mediator;

namespace TaskService.Application.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureApplicationLayer()
        {
            services.AddScoped<IDispatcher, Dispatcher>();
            services.AddScoped<CreateIssueHandler, CreateIssueHandler>();
            services.AddScoped<CreateProjectHandler, CreateProjectHandler>();
            var assembly = Assembly.GetExecutingAssembly();
            services.Scan(selector =>
                    selector.FromAssemblies(Assembly.GetExecutingAssembly())
                             .AddClasses(filter => filter.AssignableTo(typeof(IRequestHandler<,>)))
                             .AsImplementedInterfaces()
                             .WithScopedLifetime());
            //services.AddScoped<ICommandHandler<CreateUserCommand, CreateUserResult>, CreateUserHandler>();
        }
    }
}
