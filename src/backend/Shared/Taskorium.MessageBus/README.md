# 🐰 Taskorium.MessageBus.RabbitMq — Интеграция с RabbitMQ

Этот модуль предоставляет готовое решение для работы с RabbitMQ в .NET-приложениях на основе `IHostedService` и
`Microsoft.Extensions.DependencyInjection`. Поддерживает режимы публикации (publisher), потребления (consumer) и
комбинированный режим.

## 🧩 Регистрация сервисов

Модуль предоставляет методы расширения для `IServiceCollection`.

### 1. Только Publisher

Для сервисов, которые только отправляют события:

```csharp
services.AddRabbitMqPublisher(configuration);
```

### 2. Только Consumer

Для сервисов, которые только принимают и обрабатывают события:

```csharp
services.AddRabbitMqConsumer<YourIntegrationEvent, YourIntegrationEventHandler>(
    configuration, "subscription-name");
```

> Обработчик (`YourIntegrationEventHandler`) должен реализовывать интерфейс `IEventHandler<TEvent>`.

### 3. Publisher + Consumer

Оба режима можно использовать одновременно:

```csharp
services.AddRabbitMqPublisher(configuration);
services.AddRabbitMqConsumer<AnotherEvent, AnotherEventHandler>(configuration, "another-subscription");
```

> Общие сервисы (например, сериализатор, опции) регистрируются один раз благодаря `TryAddSingleton`.

## ⚙️ Конфигурация (appsettings.json)

### Полная конфигурация (Publisher + Consumer)

```json
{
  "MessageBus": {
    "RabbitMq": {
      "Connection": {
        "HostName": "localhost",
        "Port": 5672,
        "UserName": "user",
        "Password": "your-password",
        "VirtualHost": "/",
        "ClientProvidedName": "giving-to-customer-promocode-api:consumer",
        "AutomaticRecoveryEnabled": true,
        "TopologyRecoveryEnabled": true,
        "ConsumerDispatchConcurrency": 1
      },
      "Publish": {},
      "Subscriptions": {
        "giving-to-customer-promocode-received": {
          "Exchange": "receive-promocode.events",
          "ExchangeType": "topic",
          "Queue": "giving-to-customer-promocode-received",
          "RoutingKey": "promocode.received",
          "Durable": true,
          "Exclusive": false,
          "AutoDelete": false,
          "PrefetchCount": 10,
          "EnableDeadLetter": true,
          "DeadLetterExchange": "giving-to-customer-promocode.dlx",
          "DeadLetterQueue": "giving-to-customer-promocode.dlq",
          "DeadLetterRoutingKey": "giving-to-customer-promocode.dlq",
          "EnableRetry": true,
          "RetryExchange": "giving-to-customer-promocode.retry.exchange",
          "RetryQueue": "giving-to-customer-promocode.retry",
          "RetryRoutingKey": "giving-to-customer-promocode.retry",
          "RetryTtlMilliseconds": 30000,
          "MaxRetryCount": 3
        }
      }
    }
  }
}
```

> **Примечание:** Такая конфигурация использует секцию `MessageBus` и содержит полные настройки потребителя с retry, DLX
> и prefetch.

### Только Publisher — Расширенная конфигурация с настройками подключения и публикации

Более детальная конфигурация с параметрами восстановления и настройками публикации:

```json
{
  "MessageBus": {
    "RabbitMq": {
      "Connection": {
        "HostName": "localhost",
        "Port": 5672,
        "UserName": "user",
        "Password": "your-password",
        "VirtualHost": "/",
        "ClientProvidedName": "recieving-promocode-from-partners-api:publisher",
        "AutomaticRecoveryEnabled": true,
        "TopologyRecoveryEnabled": true,
        "ConsumerDispatchConcurrency": 1
      },
      "Publish": {
        "PromoCodeReceivingIntegrationEvent": {
          "Exchange": "receive-promocode.events",
          "ExchangeType": "topic",
          "RoutingKey": "promocode.received",
          "Durable": true,
          "Mandatory": true
        },
        "PartnerManagerReceivingIntegrationEvent": {
          "Exchange": "receive-promocode.events",
          "ExchangeType": "topic",
          "RoutingKey": "partner-manager.received",
          "Durable": true,
          "Mandatory": true
        }
      },
      "Subscriptions": {}
    }
  }
}
```

> **Примечание:** Такая конфигурация предполагает, что в коде используется секция
`configuration.GetSection("MessageBus")` при регистрации сервисов.

### Только Consumer

Для сервиса, который только обрабатывает события, необходимо указать параметры подписки:

```json
{
  "MessageBus": {
    "RabbitMq": {
      "Connection": {
        "HostName": "localhost",
        "Port": 5672,
        "UserName": "user",
        "Password": "your-password",
        "VirtualHost": "/",
        "ClientProvidedName": "giving-to-customer-promocode-api:consumer",
        "AutomaticRecoveryEnabled": true,
        "TopologyRecoveryEnabled": true,
        "ConsumerDispatchConcurrency": 1
      },
      "Publish": {},
      "Subscriptions": {
        "giving-to-customer-promocode-received": {
          "Exchange": "receive-promocode.events",
          "ExchangeType": "topic",
          "Queue": "giving-to-customer-promocode-received",
          "RoutingKey": "promocode.received",
          "Durable": true,
          "Exclusive": false,
          "AutoDelete": false,
          "PrefetchCount": 10,
          "EnableDeadLetter": true,
          "DeadLetterExchange": "giving-to-customer-promocode.dlx",
          "DeadLetterQueue": "giving-to-customer-promocode.dlq",
          "DeadLetterRoutingKey": "giving-to-customer-promocode.dlq",
          "EnableRetry": true,
          "RetryExchange": "giving-to-customer-promocode.retry.exchange",
          "RetryQueue": "giving-to-customer-promocode.retry",
          "RetryRoutingKey": "giving-to-customer-promocode.retry",
          "RetryTtlMilliseconds": 30000,
          "MaxRetryCount": 3
        }
      }
    }
  }
}
```

> **Примечание:** Конфигурация использует секцию `MessageBus` для единообразия и содержит полные настройки потребителя,
> включая retry, DLX, prefetch и другие продвинутые параметры.

### Параметры конфигурации

| Параметр                                  | Описание                                         | Обязательный          |
|-------------------------------------------|--------------------------------------------------|-----------------------|
| `Connection:HostName`                     | Адрес сервера RabbitMQ                           | Да                    |
| `Connection:Port`                         | Порт подключения                                 | Нет (по умолч. 5672)  |
| `Connection:UserName`                     | Имя пользователя                                 | Да                    |
| `Connection:Password`                     | Пароль                                           | Да                    |
| `Connection:VirtualHost`                  | Виртуальный хост                                 | Нет (по умолч. "/")   |
| `Connection:ClientProvidedName`           | Имя клиента в RabbitMQ                           | Нет                   |
| `Connection:AutomaticRecoveryEnabled`     | Автовосстановление соединения                    | Нет                   |
| `Connection:TopologyRecoveryEnabled`      | Автовосстановление топологии                     | Нет                   |
| `Connection:ConsumerDispatchConcurrency`  | Количество одновременно обрабатываемых сообщений | Нет                   |
| `Publish:EventName:Exchange`              | Обменник для публикации события                  | Да для publisher      |
| `Publish:EventName:ExchangeType`          | Тип обменника                                    | Нет (по умолч. topic) |
| `Publish:EventName:RoutingKey`            | Ключ маршрутизации                               | Да для publisher      |
| `Publish:EventName:Durable`               | Долговечность сообщения                          | Нет                   |
| `Publish:EventName:Mandatory`             | Обязательная доставка                            | Нет                   |
| `Subscriptions:Name:Exchange`             | Обменник для подписки                            | Да для consumer       |
| `Subscriptions:Name:Queue`                | Очередь для подписки                             | Да для consumer       |
| `Subscriptions:Name:RoutingKey`           | Ключ маршрутизации                               | Да для consumer       |
| `Subscriptions:Name:Durable`              | Долговечность очереди                            | Нет                   |
| `Subscriptions:Name:Exclusive`            | Эксклюзивность очереди                           | Нет                   |
| `Subscriptions:Name:AutoDelete`           | Автоудаление очереди                             | Нет                   |
| `Subscriptions:Name:PrefetchCount`        | Количество предзагруженных сообщений             | Нет                   |
| `Subscriptions:Name:EnableDeadLetter`     | Включить dead-letter                             | Нет                   |
| `Subscriptions:Name:DeadLetterExchange`   | DLX для сообщений                                | При включённом DLX    |
| `Subscriptions:Name:DeadLetterQueue`      | DLQ для сообщений                                | При включённом DLX    |
| `Subscriptions:Name:DeadLetterRoutingKey` | Ключ маршрутизации DLQ                           | При включённом DLX    |
| `Subscriptions:Name:EnableRetry`          | Включить retry                                   | Нет                   |
| `Subscriptions:Name:RetryExchange`        | Обменник для retry                               | При включённом retry  |
| `Subscriptions:Name:RetryQueue`           | Очередь для retry                                | При включённом retry  |
| `Subscriptions:Name:RetryRoutingKey`      | Ключ маршрутизации для retry                     | При включённом retry  |
| `Subscriptions:Name:RetryTtlMilliseconds` | TTL для retry сообщений                          | При включённом retry  |
| `Subscriptions:Name:MaxRetryCount`        | Максимальное количество повторов                 | При включённом retry  |

> **Важно:** Для потребителя (`consumer`) все параметры подписки обязательны. При их отсутствии выбрасывается
`InvalidOperationException`.

## 🛠 Архитектурные особенности

- **Сериализация:** Используется `SystemTextJsonEventSerializer`.
- **Топология:** Управление очередями и обменниками — через `IEventTopologyRegistry`.
- **Запуск потребителя:** Реализован через `IHostedService` (`EventBusHostedService`), автоматически стартует при
  запуске приложения.
- **Валидация конфигурации:** Выполняется при старте приложения (`ValidateOnStart()`).

## 🧯 Обработка ошибок

- При неверной конфигурации подписки — `InvalidOperationException` с описанием.
- Ошибки подключения и отправки — логируются через `ILogger`.
- Автоматическое восстановление соединения (реализуется в `RabbitMqEventBus`).

## 📤 Публикация событий через IEventBus

После регистрации `AddRabbitMqPublisher`, интерфейс `IEventBus` становится доступен через DI. Для публикации событий:

1. Добавьте `IEventBus` в конструктор вашего сервиса
2. Вызовите метод `PublishAsync` с экземпляром события

### Пример публикации события

```csharp
public class PromoCodeService
{
    private readonly IEventBus _eventBus;

    public PromoCodeService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task CreatePromoCodeAsync(string code, CancellationToken ct = default)
    {
        // Логика создания промокода...
        
        var @event = new PromoCodeReceivingIntegrationEvent(
            Code: code,
            IssuedAt: DateTime.UtcNow);
        
        await _eventBus.PublishAsync(@event, ct);
    }
}
```

> **Примечание:** Событие `PromoCodeReceivingIntegrationEvent` должно быть правильно сконфигурировано в секции `Publish`
> конфигурации, чтобы указать exchange, routing key и другие параметры.

### Определение события

Событие должно быть наследником `IntegrationEvent`:

```csharp
public record PromoCodeReceivingIntegrationEvent(
    string Code,
    DateTime IssuedAt) : IntegrationEvent;
```

## 📄 Лицензия

MIT © Taskorium Team