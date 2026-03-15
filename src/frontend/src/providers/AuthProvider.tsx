import React from 'react';
import {useAuth} from 'react-oidc-context';
import { HeaderAuthorization } from '../components/HeaderAuthorization';
import {useCreateUser} from '../hooks/useCreateUser';
import {useUserFullName} from '../hooks/useUserFullName';
import {AuthProviderProps, AuthInfo} from "../types";

export const AuthProvider: React.FC<AuthProviderProps> = ({
                                                              children,
                                                              activeTab,
                                                              onTabChange,
                                                              showHeader = true,
                                                          }) => {
    const auth = useAuth();
    const {syncStatus, syncError} = useCreateUser(); // авто-синхронизация
    const userFullName = useUserFullName();

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

    // Показываем контент только если синхронизация успешна или еще не начата
    const authInfo: AuthInfo = {
        isAuthenticated: auth.isAuthenticated,
        userFullName,
        onLogin: () => {
        },
        onLogout: handleLogout,
    };

    return (
        <>
            {showHeader && auth.isAuthenticated && activeTab && (
                <HeaderAuthorization
                    activeTab={activeTab as any}
                    onTabChange={onTabChange as any}
                    authInfo={authInfo}
                />
            )}
            {children}
        </>
    );
};