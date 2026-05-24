// ──────────────────────────────────────────────────────────────────────────────
// Типы SignalR-уведомлений (зеркало серверных record'ов)
// ──────────────────────────────────────────────────────────────────────────────

export type NotificationEventType =
  | 'IssueCreated'
  // Будущие события:
  // | 'IssueAssigned'
  // | 'IssueStatusChanged'
  // | 'CommentAdded'
  ;

export interface IssueCreatedPayload {
  issueId: string;
  issueKey: string;
  issueName: string;
  projectId: string;
  projectName: string;
}

// Дискриминированный union для payload в зависимости от типа события
export type NotificationPayload = IssueCreatedPayload; // расширить при добавлении событий

export interface HubNotificationMessage {
  eventType: NotificationEventType;
  title: string;
  body: string;
  payload: NotificationPayload;
  timestamp: string; // ISO DateTimeOffset
}

// Клиентское представление уведомления (с id и флагом прочтения)
export interface AppNotification extends HubNotificationMessage {
  id: string;        // генерируется на клиенте при получении
  isRead: boolean;
}
