import React, {createContext, useContext, useEffect} from 'react';
import {useAuth} from 'react-oidc-context';
import {useCreateUser} from '../hooks/useCreateUser';
import {useUserFullName} from '../hooks/useUserFullName';
import {AuthInfo} from "../types";
import {setTokenProvider} from '../api/axios';
import {signalRService} from '../api/signalRService';
import {useNotifications} from '../context/NotificationContext';

// Создаем контекст аутентификации
const AuthContext = createContext<AuthInfo | null>(null);

// Хук для использования контекста аутентификации
export const useAuthContext = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuthContext must be used within an AuthProvider');
    }
    return context;
};

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({children}) => {
    const auth = useAuth();
    const {syncStatus, syncError} = useCreateUser(); // авто-синхронизация
    const userFullName = useUserFullName();
    const {addNotification} = useNotifications();

    // Установка провайдера токена для всех API-запросов
    useEffect(() => {
        const token = auth.user?.access_token || null;
        setTokenProvider(() => token);
    }, [auth.user]);

    // Управление SignalR-соединением: подключаем после авторизации, отклю��аем при выходе
    useEffect(() => {
        if (auth.isAuthenticated && auth.user?.access_token) {
            signalRService.start(
                () => auth.user?.access_token ?? null,
                (message) => addNotification(message)
            );
        } else {
            signalRService.stop();
        }

        return () => {
            // При размонтировании провайдера (логаут / перезагрузка) — закрываем соединение
            signalRService.stop();
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [auth.isAuthenticated, auth.user?.access_token]);

    const handleLogout = () => auth.signoutRedirect();

    // Показываем загрузку пока синхронизация в процессе
    if (auth.isAuthenticated && syncStatus === 'loading') {
        return (
            <div className="sync-loading">
                <div>Загрузка данных профиля...</div>
            </div>
        );
    }

    // Показываем ошибку если синхронизация не удалась
    if (auth.isAuthenticated && syncStatus === 'error') {
        return (
            <div className="sync-error">
                <h2>Ошибка синхронизации</h2>
                <p>{syncError}</p>
                <button onClick={handleLogout}>Выйти и повторить</button>
            </div>
        );
    }

    // Создаем authInfo для контекста
    const authInfo: AuthInfo = {
        isAuthenticated: auth.isAuthenticated,
        userFullName,
        onLogin: () => {
        },
        onLogout: handleLogout,
    };

    return (
        <AuthContext.Provider value={authInfo}>
            {children}
        </AuthContext.Provider>
    );
};
