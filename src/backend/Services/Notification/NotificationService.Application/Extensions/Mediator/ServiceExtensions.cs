using Microsoft.Extensions.DependencyInjection;

namespace NotificationService.Application.Extensions.Mediator;

internal static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        internal void ConfigureMediator()
        {
            serviceCollection.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Transient);
        }
    }
}
