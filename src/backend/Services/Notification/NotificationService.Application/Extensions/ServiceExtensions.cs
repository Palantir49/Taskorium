using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Extensions.Mapping;
using NotificationService.Application.Extensions.Mediator;

namespace NotificationService.Application.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public void ConfigureApplicationLayer()
        {
            serviceCollection.ConfigureMediator();
            serviceCollection.ConfigureMapping();
        }
    }
}
