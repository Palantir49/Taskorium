# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Added

#### Backend — SignalR real-time notifications (`feature/add-signalR-communication`)

- **`TaskService.Infrastructure/Hubs/INotificationHubClient.cs`** — типизированный интерфейс SignalR-клиента `INotificationHubClient` с методом `ReceiveNotification(HubNotificationMessage)` и record-сообщением `HubNotificationMessage(EventType, Title, Body, Payload, Timestamp)`.
- **`TaskService.Application/Notifications/IIssueNotificationService.cs`** — интерфейс уведомлений на уровне Application; изолирует бизнес-логику от инфраструктуры SignalR.
- **`TaskService.Infrastructure/Services/IssueNotificationService.cs`** — реализация `IIssueNotificationService` через `IHubContext<NotificationHub, INotificationHubClient>`; содержит record `IssueCreatedPayload` и класс констант `NotificationEventTypes` для будущих типов событий.

#### Frontend — UI уведомлений

- **`src/types/notification.ts`** — TypeScript-типы, зеркалящие серверные record'ы: `HubNotificationMessage`, `AppNotification`, `IssueCreatedPayload`, `NotificationEventType`.
- **`src/api/signalRService.ts`** — синглтон `SignalRService`: управляет жизненным циклом соединения с хабом, передаёт JWT через `accessTokenFactory`, реализует экспоненциальный реконнект (2 с → 5 с → 10 с → 30 с).
- **`src/context/NotificationContext.tsx`** — `NotificationProvider` с reducer-based состоянием; предоставляет `addNotification`, `markRead`, `markAllRead`, `removeNotification`, `clearAll` и `unreadCount`.
- **`src/components/NotificationBell.tsx`** — `<NotificationBell>` — дропдаун с бейджем непрочитанных, действиями «прочитать» / «удалить» для каждого элемента; `<NotificationToastContainer>` — всплывающие уведомления с авто-скрытием через 5 секунд.
- **`src/components/NotificationBell.css`** — стили колокольчика, дропдауна, элементов списка и тост-контейнера.

### Changed

#### Backend

- **`TaskService.Infrastructure/Hubs/NotificationHub.cs`** — базовый класс изменён с `Hub` на `Hub<INotificationHubClient>` (типизированный хаб).
- **`TaskService.Application/Features/Issues/Handler/IssueCreateHandler.cs`** — после успешного `SaveChangesAsync` выполняет JOIN `ProjectMembers → Users` для получения Keycloak ID участников с ролями `Creator` / `Admin`, объединяет с Keycloak ID текущего пользователя (исполнителя), дедуплицирует и вызывает `IIssueNotificationService.NotifyIssueCreatedAsync`. Сбой доставки перехватывается (try/catch) и не прерывает основную операцию.
- **`TaskService.Infrastructure/Extensions/ServiceExtensions.cs`** — зарегистрирован `IssueNotificationService` как `IIssueNotificationService` с временем жизни Scoped.
- **`TaskService.Infrastructure/TaskService.Infrastructure.csproj`** — добавлен `<FrameworkReference Include="Microsoft.AspNetCore.App" />` для доступа к SignalR-типам из проекта class library.
- **`TaskService.Api/Program.cs`** — добавлен `.AllowCredentials()` в CORS-политику `AllowReactApp` (обязательно для long-polling и SSE транспортов SignalR).

#### Frontend

- **`src/providers/AuthProvider.tsx`** — при `auth.isAuthenticated = true` запускает SignalR-соединение; останавливает при выходе или размонтировании провайдера; входящие сообщения `ReceiveNotification` передаёт в `NotificationContext.addNotification`.
- **`src/App.tsx`** — аутентифицированный контент обёрнут в `<NotificationProvider>`; добавлен `<NotificationToastContainer />` внутри дерева провайдера.
- **`src/components/HeaderKanbanBoard.tsx`** — добавлен `<NotificationBell />` рядом с блоком профиля пользователя в шапке.
- **`vite.config.js`** — добавлена прокси-запись `/hubs` с `ws: true` для поддержки WebSocket при локальной разработке.
- **`package.json`** — добавлена зависимость `@microsoft/signalr ^8.0.7`.

---

## Previous releases

<!-- Релизы до введения этого changelog не документированы. -->
