using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;
using NotificationService.Infrastructure.Persistence.Mongo.Options;

namespace NotificationService.Infrastructure.Extensions.Persistence.Mongo;

internal static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        internal void AddMongo(IConfiguration configuration)
        {
            var mongoDbOptions = configuration
                .GetSection(MongoDbOptions.SectionName)
                .Get<MongoDbOptions>();

            ArgumentNullException.ThrowIfNull(mongoDbOptions);

            var credential = MongoCredential.CreateCredential(mongoDbOptions.AuthSource, mongoDbOptions.Username,
                mongoDbOptions.Password);

            serviceCollection.AddSingleton<IMongoClient>(_ =>
            {
                var clientSettings = new MongoClientSettings
                {
                    Credential = credential,
                    Server = new MongoServerAddress(mongoDbOptions.Host, mongoDbOptions.Port)
                };

                return new MongoClient(clientSettings);
            });

            serviceCollection.AddScoped<NotificationDbContext>();
        }
    }
}
