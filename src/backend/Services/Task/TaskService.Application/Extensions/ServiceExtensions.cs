using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Mediator;

namespace TaskService.Application.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureApplicationLayer()
        {
            services.AddScoped<IDispatcher, Dispatcher>();
            services.Scan(selector =>
                selector.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(filter => filter.AssignableTo(typeof(IRequestHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
