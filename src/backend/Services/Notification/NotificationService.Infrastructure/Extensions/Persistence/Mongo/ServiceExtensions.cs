using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using NotificationService.Infrastructure.Persistence.Mongo.Configurations;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;
using NotificationService.Infrastructure.Persistence.Mongo.Indexes;
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
            MongoConfiguration.Configure();
            serviceCollection.AddSingleton<IMongoClient>(_ =>
            {
                var clientSettings = new MongoClientSettings
                {
                    Credential = credential,
                    Server = new MongoServerAddress(mongoDbOptions.Host, mongoDbOptions.Port),
                    ReplicaSetName = mongoDbOptions.ReplicaSet,
                    DirectConnection = true
                };

                return new MongoClient(clientSettings);
            });
            BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            serviceCollection.AddScoped<NotificationDbContext>();
            serviceCollection.AddHostedService<IndexInitializer>();
        }
    }
}
