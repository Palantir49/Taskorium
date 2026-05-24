import React, { createContext, useCallback, useContext, useReducer } from 'react';
import { AppNotification, HubNotificationMessage } from '../types/notification';

// ──────────────────────────────────────────────────────────────────────────────
// State & Actions
// ──────────────────────────────────────────────────────────────────────────────

interface NotificationState {
  notifications: AppNotification[];
}

type NotificationAction =
  | { type: 'ADD'; message: HubNotificationMessage }
  | { type: 'MARK_READ'; id: string }
  | { type: 'MARK_ALL_READ' }
  | { type: 'REMOVE'; id: string }
  | { type: 'CLEAR_ALL' };

function reducer(state: NotificationState, action: NotificationAction): NotificationState {
  switch (action.type) {
    case 'ADD':
      return {
        notifications: [
          {
            ...action.message,
            id: crypto.randomUUID(),
            isRead: false,
          },
          ...state.notifications,
        ],
      };
    case 'MARK_READ':
      return {
        notifications: state.notifications.map((n) =>
          n.id === action.id ? { ...n, isRead: true } : n
        ),
      };
    case 'MARK_ALL_READ':
      return { notifications: state.notifications.map((n) => ({ ...n, isRead: true })) };
    case 'REMOVE':
      return { notifications: state.notifications.filter((n) => n.id !== action.id) };
    case 'CLEAR_ALL':
      return { notifications: [] };
    default:
      return state;
  }
}

// ──────────────────────────────────────────────────────────────────────────────
// Context
// ──────────────────────────────────────────────────────────────────────────────

interface NotificationContextValue {
  notifications: AppNotification[];
  unreadCount: number;
  addNotification: (message: HubNotificationMessage) => void;
  markRead: (id: string) => void;
  markAllRead: () => void;
  removeNotification: (id: string) => void;
  clearAll: () => void;
}

const NotificationContext = createContext<NotificationContextValue | undefined>(undefined);

export function NotificationProvider({ children }: { children: React.ReactNode }) {
  const [state, dispatch] = useReducer(reducer, { notifications: [] });

  const addNotification = useCallback((message: HubNotificationMessage) => {
    dispatch({ type: 'ADD', message });
  }, []);

  const markRead = useCallback((id: string) => dispatch({ type: 'MARK_READ', id }), []);
  const markAllRead = useCallback(() => dispatch({ type: 'MARK_ALL_READ' }), []);
  const removeNotification = useCallback((id: string) => dispatch({ type: 'REMOVE', id }), []);
  const clearAll = useCallback(() => dispatch({ type: 'CLEAR_ALL' }), []);

  const value: NotificationContextValue = {
    notifications: state.notifications,
    unreadCount: state.notifications.filter((n) => !n.isRead).length,
    addNotification,
    markRead,
    markAllRead,
    removeNotification,
    clearAll,
  };

  return (
    <NotificationContext.Provider value={value}>
      {children}
    </NotificationContext.Provider>
  );
}

export function useNotifications(): NotificationContextValue {
  const ctx = useContext(NotificationContext);
  if (!ctx) throw new Error('useNotifications must be used within NotificationProvider');
  return ctx;
}
