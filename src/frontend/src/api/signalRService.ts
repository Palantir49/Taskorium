import * as signalR from '@microsoft/signalr';
import { HubNotificationMessage } from '../types/notification';

const HUB_URL = '/hubs/notifications';

type NotificationHandler = (message: HubNotificationMessage) => void;

class SignalRService {
  private connection: signalR.HubConnection | null = null;

  /**
   * Создаёт и запускает SignalR-соединение.
   * @param getToken  Фабрика JWT-токена; вызывается SignalR при каждом подключении/реконнекте.
   * @param onNotification  Callback для полученных уведомлений.
   */
  async start(getToken: () => string | null, onNotification: NotificationHandler): Promise<void> {
    if (this.connection) {
      await this.stop();
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(HUB_URL, {
        // SignalR передаёт токен через query-string ?access_token=...
        // Сервер уже настроен читать его в OnMessageReceived JwtBearerEvents
        accessTokenFactory: () => getToken() ?? '',
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (ctx) => {
          // Экспоненциальная задержка: 2s, 5s, 10s, 30s, затем null (прекратить)
          const delays = [2000, 5000, 10000, 30000];
          return delays[ctx.previousRetryCount] ?? null;
        },
      })
      .configureLogging(
        import.meta.env.DEV
          ? signalR.LogLevel.Information
          : signalR.LogLevel.Warning
      )
      .build();

    this.connection.on('ReceiveNotification', onNotification);

    this.connection.onreconnecting(() => {
      console.log('[SignalR] Reconnecting...');
    });
    this.connection.onreconnected(() => {
      console.log('[SignalR] Reconnected');
    });
    this.connection.onclose((err) => {
      if (err) console.error('[SignalR] Connection closed with error:', err);
    });

    try {
      await this.connection.start();
      console.log('[SignalR] Connected, connectionId:', this.connection.connectionId);
    } catch (err) {
      console.error('[SignalR] Failed to start connection:', err);
    }
  }

  async stop(): Promise<void> {
    if (this.connection) {
      this.connection.off('ReceiveNotification');
      await this.connection.stop();
      this.connection = null;
    }
  }

  get state(): signalR.HubConnectionState {
    return this.connection?.state ?? signalR.HubConnectionState.Disconnected;
  }
}

// Синглтон — одно соединение на всё приложение
export const signalRService = new SignalRService();
