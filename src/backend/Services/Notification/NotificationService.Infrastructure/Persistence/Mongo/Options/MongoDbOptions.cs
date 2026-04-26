namespace NotificationService.Infrastructure.Persistence.Mongo.Options;

public record MongoDbOptions
{
    public const string SectionName = "Persistence:MongoDb";
    public required string Host { get; init; }

    public required int Port { get; init; }

    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string AuthSource { get; init; }
    public required string Database { get; init; }
}
