using System.Reflection;
using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using NotificationService.Domain.Aggregates.Notification;
using NotificationService.Domain.Aggregates.Outbox;
using NotificationService.Domain.Enums;
using NotificationService.Domain.ValueObjects;
using NotificationService.Infrastructure.Persistence.Mongo.Serializers;

namespace NotificationService.Infrastructure.Persistence.Mongo.Configurations;

public static class MongoConfiguration
{
    private static bool _configured;
    private static readonly Lock Lock = new();

    public static void Configure()
    {
        lock (Lock)
        {
            if (_configured)
            {
                return;
            }

            RegisterConventions();
            RegisterSerializers();
            RegisterClassMaps();

            _configured = true;
        }
    }

    // ── Conventions ───────────────────────────────────────────────────────────

    private static void RegisterConventions()
    {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true), // не падать на лишних полях
            new EnumRepresentationConvention(BsonType.String) // enum как строка
        };

        ConventionRegistry.Register(
            "NotificationServiceConventions", pack, _ => true);
    }

    // ── Serializers для Value Objects ─────────────────────────────────────────

    private static void RegisterSerializers()
    {
        // IdempotencyKey — record с приватным конструктором
        // Сериализуем как plain string чтобы не было { "Value": "..." }
        BsonSerializer.RegisterSerializer(new IdempotencyKeySerializer());

        // RecipientId — record с приватным конструктором
        BsonSerializer.RegisterSerializer(new RecipientIdSerializer());

        BsonSerializer.RegisterSerializer(
            new EnumSerializer<NotificationStatus>(BsonType.String));
        BsonSerializer.RegisterSerializer(
            new EnumSerializer<RecipientNotificationStatus>(BsonType.String));
        BsonSerializer.RegisterSerializer(
            new EnumSerializer<ChannelType>(BsonType.String));
        BsonSerializer.RegisterSerializer(
            new EnumSerializer<ChannelStatus>(BsonType.String));
    }

    // ── ClassMaps ─────────────────────────────────────────────────────────────

    private static void RegisterClassMaps()
    {
        RegisterNotificationClassMap();
        RegisterRecipientNotificationClassMap();
        RegisterNotificationChannelClassMap();
        RegisterNotificationContentClassMap();
        RegisterRecipientClassMap();
        RegisterOutboxMessageClassMap();
    }

    // ─── Notification ─────────────────────────────────────────────────────────

    private static void RegisterNotificationClassMap()
    {
        BsonClassMap.RegisterClassMap<Notification>(cm =>
        {
            cm.SetCreator(() =>
            {
                var obj = (Notification)RuntimeHelpers
                    .GetUninitializedObject(typeof(Notification));

                // ← инициализируем вручную через рефлексию
                // потому что GetUninitializedObject не вызывает конструктор
                typeof(Notification)
                    .GetProperty(nameof(Notification.RecipientNotifications),
                        BindingFlags.Public | BindingFlags.Instance)!
                    .SetValue(obj, new List<RecipientNotification>());

                return obj;
            });


            // EventIdempotencyKey — record с приватным конструктором
            // IdempotencyKeySerializer сериализует его как plain string
            cm.MapProperty(n => n.EventIdempotencyKey)
                .SetElementName("EventIdempotencyKey");

            cm.MapProperty(n => n.Status)
                .SetElementName("Status");

            // DateTimeOffset → MongoDB хранит корректно
            cm.MapProperty(n => n.CreatedAtUtc)
                .SetElementName("CreatedAtUtc");

            cm.MapProperty(n => n.CompletedAt)
                .SetElementName("CompletedAt");

            cm.MapProperty(n => n.RecipientNotifications)
                .SetElementName("RecipientNotifications");
        });
    }

    // ─── RecipientNotification ────────────────────────────────────────────────

    private static void RegisterRecipientNotificationClassMap()
    {
        BsonClassMap.RegisterClassMap<RecipientNotification>(cm =>
        {
            cm.SetCreator(() =>
                (RecipientNotification)RuntimeHelpers
                    .GetUninitializedObject(typeof(RecipientNotification)));

            // Recipient — record с приватным конструктором
            // RegisterRecipientClassMap обрабатывает его
            cm.MapProperty(r => r.Recipient)
                .SetElementName("Recipient");

            cm.MapProperty(r => r.Content)
                .SetElementName("Content");

            cm.MapProperty(r => r.Status)
                .SetElementName("Status");

            // Приватное поле _channels
            cm.MapProperty(r => r.Channels)
                .SetElementName("Channels");
        });
    }

    // ─── NotificationChannel ──────────────────────────────────────────────────

    private static void RegisterNotificationChannelClassMap()
    {
        BsonClassMap.RegisterClassMap<NotificationChannel>(cm =>
        {
            cm.SetCreator(() =>
                (NotificationChannel)RuntimeHelpers
                    .GetUninitializedObject(typeof(NotificationChannel)));

            cm.MapProperty(c => c.Type).SetElementName("Type");
            cm.MapProperty(c => c.Status).SetElementName("Status");
            cm.MapProperty(c => c.Address).SetElementName("Address");
            cm.MapProperty(c => c.ErrorMessage).SetElementName("ErrorMessage");
            cm.MapProperty(c => c.SentAt).SetElementName("SentAt");
            cm.MapProperty(c => c.LastAttemptAt).SetElementName("LastAttemptAt");
            cm.MapProperty(c => c.RetryCount).SetElementName("RetryCount");
            cm.MapProperty(c => c.MaxRetries).SetElementName("MaxRetries");
        });
    }

    // ─── NotificationContent ──────────────────────────────────────────────────

    private static void RegisterNotificationContentClassMap()
    {
        // record — нет приватного конструктора без параметров,
        // но все свойства init-only → GetUninitializedObject + рефлексия
        BsonClassMap.RegisterClassMap<NotificationContent>(cm =>
        {
            cm.SetCreator(() =>
                (NotificationContent)RuntimeHelpers
                    .GetUninitializedObject(typeof(NotificationContent)));

            cm.MapProperty(c => c.Subject).SetElementName("Subject");
            cm.MapProperty(c => c.Body).SetElementName("Body");
            cm.MapProperty(c => c.ActionUrl).SetElementName("ActionUrl");
            cm.MapProperty(c => c.Metadata).SetElementName("Metadata");
        });
    }

    // ─── Recipient ────────────────────────────────────────────────────────────

    private static void RegisterRecipientClassMap()
    {
        // record с приватным конструктором
        BsonClassMap.RegisterClassMap<Recipient>(cm =>
        {
            cm.SetCreator(() =>
                (Recipient)RuntimeHelpers
                    .GetUninitializedObject(typeof(Recipient)));

            // Id — RecipientId, сериализуется через RecipientIdSerializer как string
            cm.MapProperty(r => r.Id).SetElementName("Id");
            cm.MapProperty(r => r.UserId).SetElementName("UserId");
            cm.MapProperty(r => r.FullName).SetElementName("FullName");
            cm.MapProperty(r => r.Email).SetElementName("Email");
            cm.MapProperty(r => r.Phone).SetElementName("Phone");
            cm.MapProperty(r => r.IsMuted).SetElementName("IsMuted");

            // IReadOnlyCollection<ChannelType> — MongoDB сериализует как массив строк
            // благодаря EnumRepresentationConvention(BsonType.String)
            cm.MapProperty(r => r.PreferredChannels)
                .SetElementName("PreferredChannels");
        });
    }

    // ─── OutBoxMessage ────────────────────────────────────────────────────────

    private static void RegisterOutboxMessageClassMap()
    {
        BsonClassMap.RegisterClassMap<OutBoxMessage>(cm =>
        {
            cm.SetCreator(() =>
                (OutBoxMessage)RuntimeHelpers
                    .GetUninitializedObject(typeof(OutBoxMessage)));


            cm.MapProperty(o => o.NotificationId).SetElementName("NotificationId");
            cm.MapProperty(o => o.CreatedAt).SetElementName("CreatedAt");
            cm.MapProperty(o => o.ProcessedAt).SetElementName("ProcessedAt");
            cm.MapProperty(o => o.Processed).SetElementName("Processed");
            cm.MapProperty(o => o.RetryCount).SetElementName("RetryCount");
            cm.MapProperty(o => o.LastError).SetElementName("LastError");
            cm.MapProperty(o => o.LockedAt).SetElementName("LockedAt");
            cm.MapProperty(o => o.LockedBy).SetElementName("LockedBy");
        });
    }
}
