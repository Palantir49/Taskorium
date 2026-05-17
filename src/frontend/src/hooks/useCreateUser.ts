import {useAuth} from 'react-oidc-context';
import {useEffect, useRef, useState} from 'react';
import axios from 'axios';

// Константы для ключей localStorage
const STORAGE_KEYS = {
    SYNCED_USERS: 'synced_users',
    SYNC_ERRORS: 'sync_errors'
} as const;

// Типы для хранения
interface SyncedUser {
    userId: string;
    timestamp: number;
    status: 'success' | 'error';
}

export const useCreateUser = () => {
    const auth = useAuth();
    const [syncStatus, setSyncStatus] = useState<'idle' | 'loading' | 'success' | 'error'>('idle');
    const [syncError, setSyncError] = useState<string | null>(null);

    // Refs для текущей сессии
    const hasSyncedRef = useRef(false);
    const syncedUserIdRef = useRef<string | null>(null);

    // Функции для работы с localStorage
    const getSyncedUsersFromStorage = (): SyncedUser[] => {
        try {
            const stored = localStorage.getItem(STORAGE_KEYS.SYNCED_USERS);
            return stored ? JSON.parse(stored) : [];
        } catch {
            return [];
        }
    };

    const addSyncedUserToStorage = (userId: string, status: 'success' | 'error') => {
        try {
            const syncedUsers = getSyncedUsersFromStorage();

            // Удаляем старую запись этого пользователя если есть
            const filteredUsers = syncedUsers.filter(user => user.userId !== userId);

            // Добавляем новую запись
            filteredUsers.push({
                userId,
                timestamp: Date.now(),
                status
            });

            // Ограничиваем количество записей (сохраняем только последние 100)
            const limitedUsers = filteredUsers.slice(-100);

            localStorage.setItem(STORAGE_KEYS.SYNCED_USERS, JSON.stringify(limitedUsers));
        } catch (error) {
            console.error('Failed to save to localStorage:', error);
        }
    };

    const removeSyncedUserFromStorage = (userId: string) => {
        try {
            const syncedUsers = getSyncedUsersFromStorage();
            const filteredUsers = syncedUsers.filter(user => user.userId !== userId);
            localStorage.setItem(STORAGE_KEYS.SYNCED_USERS, JSON.stringify(filteredUsers));
        } catch (error) {
            console.error('Failed to remove from localStorage:', error);
        }
    };

    const isUserSyncedInStorage = (userId: string): boolean => {
        try {
            const syncedUsers = getSyncedUsersFromStorage();
            const userRecord = syncedUsers.find(user => user.userId === userId);

            if (!userRecord) return false;

            // Проверяем, не устарела ли запись (старше 24 часов)
            const isExpired = Date.now() - userRecord.timestamp > 24 * 60 * 60 * 1000;

            return !isExpired && userRecord.status === 'success';
        } catch {
            return false;
        }
    };

    useEffect(() => {
        // Условия для выполнения синхронизации:
        // 1. Пользователь аутентифицирован
        // 2. Есть данные пользователя
        // 3. Еще не выполняли синхронизацию в ЭТОЙ сессии
        // 4. Пользователь не был синхронизирован в localStorage (или устарел)
        // 5. Синхронизация еще не в процессе
        const userId = auth.user?.profile.sub;

        if (!auth.isAuthenticated || !auth.user || !userId) return;

        // Проверяем в localStorage
        const isSyncedInStorage = isUserSyncedInStorage(userId);

        if (isSyncedInStorage) {
            console.log('👤 User already synced (from storage)');
            hasSyncedRef.current = true;
            syncedUserIdRef.current = userId;
            setSyncStatus('success');
            return;
        }

        const shouldSync = userId !== syncedUserIdRef.current &&
            !hasSyncedRef.current &&
            syncStatus === 'idle';

        if (!shouldSync) return;

        // Помечаем как начавшуюся синхронизацию
        hasSyncedRef.current = true;
        syncedUserIdRef.current = userId;

        const syncUser = async () => {
            setSyncStatus('loading');
            setSyncError(null);

            const userData = {
                keycloakId: userId,
                email: auth.user.profile.email,
                name: auth.user.profile.name,
                given_name: auth.user.profile.given_name,
                family_name: auth.user.profile.family_name,
                username: auth.user.profile.preferred_username,
            };

            try {
                const response = await axios.post('/api/v1/User', userData, {
                    headers: {
                        'Authorization': `Bearer ${auth.user.access_token}`,
                        'Content-Type': 'application/json',
                    }
                });

                console.log('✅ User synced to backend');

                // Сохраняем успех в localStorage
                addSyncedUserToStorage(userId, 'success');
                setSyncStatus('success');

            } catch (error) {
                console.error('❌ User sync failed:', error);

                // При ошибке разрешаем повторную попытку для этого пользователя
                hasSyncedRef.current = false;
                syncedUserIdRef.current = null;
                setSyncStatus('error');

                // Обработка ошибок по статусам
                if (axios.isAxiosError(error)) {
                    if (error.response) {
                        const status = error.response.status;

                        switch (status) {
                            case 400:
                                setSyncError('Некорректные данные пользователя');
                                addSyncedUserToStorage(userId, 'error');
                                break;
                            case 401:
                                setSyncError('Ошибка авторизации. Пожалуйста, войдите заново');
                                addSyncedUserToStorage(userId, 'error');
                                break;
                            case 403:
                                setSyncError('Недостаточно прав для создания пользователя');
                                addSyncedUserToStorage(userId, 'error');
                                break;
                            case 409:
                                console.log('Пользователь уже существует');
                                // Для 409 тоже считаем успехом
                                hasSyncedRef.current = true;
                                syncedUserIdRef.current = userId;
                                addSyncedUserToStorage(userId, 'success');
                                setSyncStatus('success');
                                return;
                            case 429:
                                setSyncError('Слишком много запросов. Попробуйте позже');
                                addSyncedUserToStorage(userId, 'error');
                                break;
                            case 500:
                                setSyncError('Внутренняя ошибка сервера');
                                addSyncedUserToStorage(userId, 'error');
                                break;
                            case 502:
                            case 503:
                            case 504:
                                setSyncError('Сервис временно недоступен');
                                addSyncedUserToStorage(userId, 'error');
                                break;
                            default:
                                setSyncError(`Ошибка сервера: ${error.response.status}`);
                                addSyncedUserToStorage(userId, 'error');
                        }
                    } else if (error.request) {
                        setSyncError('Нет ответа от сервера. Проверьте подключение');
                        addSyncedUserToStorage(userId, 'error');
                    } else {
                        setSyncError(`Ошибка: ${error.message}`);
                        addSyncedUserToStorage(userId, 'error');
                    }
                } else {
                    setSyncError('Неизвестная ошибка при синхронизации');
                    addSyncedUserToStorage(userId, 'error');
                }
            }
        };

        syncUser();
    }, [auth.isAuthenticated, auth.user, syncStatus]);

    // Сбрасываем статус при разлогине
    useEffect(() => {
        if (!auth.isAuthenticated) {
            hasSyncedRef.current = false;
            syncedUserIdRef.current = null;
            setSyncStatus('idle');
            setSyncError(null);
        }
    }, [auth.isAuthenticated]);

    // Функция для ручного повторного вызова
    const retrySync = () => {
        if (auth.isAuthenticated && auth.user) {
            const userId = auth.user.profile.sub;

            // Удаляем из localStorage чтобы можно было повторить
            removeSyncedUserFromStorage(userId);

            hasSyncedRef.current = false;
            syncedUserIdRef.current = null;
            setSyncStatus('idle');
            setSyncError(null);
        }
    };

    // Функция для полной очистки кэша
    const clearSyncCache = () => {
        try {
            localStorage.removeItem(STORAGE_KEYS.SYNCED_USERS);
            hasSyncedRef.current = false;
            syncedUserIdRef.current = null;
            setSyncStatus('idle');
            setSyncError(null);
            console.log('🧹 Sync cache cleared');
        } catch (error) {
            console.error('Failed to clear sync cache:', error);
        }
    };

    return {
        syncStatus,
        syncError,
        retrySync,
        clearSyncCache
    };
};