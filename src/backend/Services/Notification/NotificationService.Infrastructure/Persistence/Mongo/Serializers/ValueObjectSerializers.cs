using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using NotificationService.Domain.ValueObjects;

namespace NotificationService.Infrastructure.Persistence.Mongo.Serializers;

/// <summary>
///     IdempotencyKey — record с приватным конструктором.
///     Сериализуем как plain string: "3fa85f64-..."
///     Без сериализатора MongoDB пишет: { "Value": "3fa85f64-..." }
///     и LINQ фильтры по этому полю не работают.
/// </summary>
public sealed class IdempotencyKeySerializer : SerializerBase<IdempotencyKey>
{
    public override void Serialize(
        BsonSerializationContext ctx,
        BsonSerializationArgs args,
        IdempotencyKey value)
    {
        ctx.Writer.WriteString(value.Value.ToString());
    }

    public override IdempotencyKey Deserialize(
        BsonDeserializationContext ctx,
        BsonDeserializationArgs args) // Используем статический фабричный метод — приватный конструктор недоступен
    {
        return IdempotencyKey.Create(Guid.Parse(ctx.Reader.ReadString()));
    }
}

/// <summary>
///     RecipientId — record с приватным конструктором.
///     Сериализуем как plain string: "user-123"
/// </summary>
public sealed class RecipientIdSerializer : SerializerBase<RecipientId>
{
    public override void Serialize(
        BsonSerializationContext ctx,
        BsonSerializationArgs args,
        RecipientId value)
    {
        ctx.Writer.WriteString(value.Value);
    }

    public override RecipientId Deserialize(
        BsonDeserializationContext ctx,
        BsonDeserializationArgs args)
    {
        return RecipientId.FromUser(ctx.Reader.ReadString());
    }
}
