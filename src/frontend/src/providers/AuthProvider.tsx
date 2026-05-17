import React, { createContext, useContext, useEffect } from 'react';
import {useAuth} from 'react-oidc-context';
import {useCreateUser} from '../hooks/useCreateUser';
import {useUserFullName} from '../hooks/useUserFullName';
import {AuthInfo} from "../types";
import { setTokenProvider as setTaskTokenProvider } from '../api/taskService';
import { setTokenProvider as setWorkspaceTokenProvider } from '../api/workSpaceService';
import { setTokenProvider as setProjectTokenProvider } from '../api/projectService';

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

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const auth = useAuth();
    const {syncStatus, syncError} = useCreateUser(); // авто-синхронизация
    const userFullName = useUserFullName();

    // Установка провайдера токена для всех API-запросов
    useEffect(() => {
        const token = auth.user?.access_token || null;
        setTaskTokenProvider(() => token);
        setWorkspaceTokenProvider(() => token);
        setProjectTokenProvider(() => token);
    }, [auth.user]);

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
        onLogin: () => {},
        onLogout: handleLogout,
    };

    return (
        <AuthContext.Provider value={authInfo}>
            {children}
        </AuthContext.Provider>
    );
};
