using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskService.Infrastructure.Extensions.Cache;

internal static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        internal void AddCache(IConfiguration configuration)
        {
            services.AddHybridCache();
            services.AddRedis(configuration);
        }


        private void AddHybridCache()
        {
            services.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    LocalCacheExpiration = TimeSpan.FromDays(1), Expiration = TimeSpan.FromDays(7)
                };
            });
        }

        private void AddRedis(IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });
        }
    }
}
