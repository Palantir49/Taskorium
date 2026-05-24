import React, { useEffect, useRef, useState } from 'react';
import { useNotifications } from '../context/NotificationContext';
import { AppNotification } from '../types/notification';
import './NotificationBell.css';

// ──────────────────────────────────────────────────────────────────────────────
// Toast — авто-скрывающиеся уведомления (5 сек)
// ──────────────────────────────────────────────────────────────────────────────

interface ToastItem {
  id: string;
  title: string;
  body: string;
}

export function NotificationToastContainer() {
  const { notifications } = useNotifications();
  const [toasts, setToasts] = useState<ToastItem[]>([]);
  const prevCountRef = useRef(0);

  // Показываем тост при каждом новом (непрочитанном) уведомлении
  useEffect(() => {
    const unread = notifications.filter((n) => !n.isRead);
    if (unread.length > prevCountRef.current) {
      const newest = unread[0]; // addNotification кладёт в начало
      const toast: ToastItem = { id: newest.id, title: newest.title, body: newest.body };
      setToasts((prev) => [...prev, toast]);
      const timer = setTimeout(
        () => setToasts((prev) => prev.filter((t) => t.id !== toast.id)),
        5000
      );
      return () => clearTimeout(timer);
    }
    prevCountRef.current = unread.length;
  }, [notifications]);

  if (!toasts.length) return null;

  return (
    <div className="notification-toast-container">
      {toasts.map((toast) => (
        <div key={toast.id} className="notification-toast">
          <span className="notification-toast-icon">🔔</span>
          <div className="notification-toast-body">
            <div className="notification-toast-title">{toast.title}</div>
            <div className="notification-toast-text">{toast.body}</div>
          </div>
          <button
            className="notification-toast-close"
            onClick={() => setToasts((prev) => prev.filter((t) => t.id !== toast.id))}
          >
            ✕
          </button>
        </div>
      ))}
    </div>
  );
}

// ──────────────────────────────────────────────────────────────────────────────
// Bell — иконка с дропдауном
// ──────────────────────────────────────────────────────────────────────────────

function formatRelativeTime(isoString: string): string {
  const diff = Date.now() - new Date(isoString).getTime();
  const mins = Math.floor(diff / 60000);
  if (mins < 1) return 'только что';
  if (mins < 60) return `${mins} мин. назад`;
  const hours = Math.floor(mins / 60);
  if (hours < 24) return `${hours} ч. назад`;
  return `${Math.floor(hours / 24)} дн. назад`;
}

function NotificationItem({
  notification,
  onMarkRead,
  onRemove,
}: {
  notification: AppNotification;
  onMarkRead: () => void;
  onRemove: () => void;
}) {
  return (
    <div
      className={`notification-item ${notification.isRead ? '' : 'unread'}`}
      onClick={onMarkRead}
    >
      <span className={`notification-item-dot ${notification.isRead ? 'hidden' : ''}`} />
      <div className="notification-item-content">
        <div className="notification-item-title">{notification.title}</div>
        <div className="notification-item-body">{notification.body}</div>
        <div className="notification-item-time">{formatRelativeTime(notification.timestamp)}</div>
      </div>
      <button
        className="notification-item-remove"
        title="Удалить"
        onClick={(e) => {
          e.stopPropagation();
          onRemove();
        }}
      >
        ✕
      </button>
    </div>
  );
}

export function NotificationBell() {
  const { notifications, unreadCount, markRead, markAllRead, removeNotification } =
    useNotifications();
  const [open, setOpen] = useState(false);
  const wrapperRef = useRef<HTMLDivElement>(null);

  // Закрыть дропдаун при клике вне него
  useEffect(() => {
    const handler = (e: MouseEvent) => {
      if (wrapperRef.current && !wrapperRef.current.contains(e.target as Node)) {
        setOpen(false);
      }
    };
    document.addEventListener('mousedown', handler);
    return () => document.removeEventListener('mousedown', handler);
  }, []);

  return (
    <div className="notification-bell-wrapper" ref={wrapperRef}>
      <button
        className="notification-bell-btn"
        aria-label="Уведомления"
        onClick={() => setOpen((v) => !v)}
      >
        🔔
        {unreadCount > 0 && (
          <span className="notification-badge">{unreadCount > 99 ? '99+' : unreadCount}</span>
        )}
      </button>

      {open && (
        <div className="notification-dropdown">
          <div className="notification-dropdown-header">
            <h3>Уведомления</h3>
            {unreadCount > 0 && (
              <button className="notification-mark-all-btn" onClick={markAllRead}>
                Отметить все как прочитанные
              </button>
            )}
          </div>

          <div className="notification-list">
            {notifications.length === 0 ? (
              <div className="notification-empty">Нет уведомлений</div>
            ) : (
              notifications.map((n) => (
                <NotificationItem
                  key={n.id}
                  notification={n}
                  onMarkRead={() => markRead(n.id)}
                  onRemove={() => removeNotification(n.id)}
                />
              ))
            )}
          </div>
        </div>
      )}
    </div>
  );
}
