using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Infrastructure.Extensions.Cache;
using TaskService.Infrastructure.Extensions.Services.FileStorage;
using TaskService.Infrastructure.Extensions.Services.MessageBus;
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
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                    options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
                    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
                    {
                        FakeDataFactory fake = new FakeDataFactory();
                        await fake.Seed((TaskServiceDbContext)context, cancellationToken);
                    });
                    options.UseSeeding((context, _) =>
                    {
                        FakeDataFactory fake = new FakeDataFactory();
                        fake.Seed((TaskServiceDbContext)context);
                    });
                });


            services.AddCache(configuration);
            services.ConfigureGrpcFileStorageClient(configuration);
            services.AddMessageBus(configuration);
        }
    }
    extension(IServiceProvider provider)
    {
        public async Task InitializeDatabaseAsync()
        {
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TaskServiceDbContext>();

            // Автоматически вызывает все зарегистрированные UseAsyncSeeding
            await context.Database.EnsureCreatedAsync();
        }
    }
}
