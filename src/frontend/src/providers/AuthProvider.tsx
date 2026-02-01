import React from 'react';
import {useAuth} from 'react-oidc-context';
import Header from '../components/Header';
import {useCreateUser} from '../hooks/useCreateUser';
import {useUserFullName} from '../hooks/useUserFullName';
import {AuthProviderProps} from "../types";


export const AuthProvider: React.FC<AuthProviderProps> = ({
                                                              children,
                                                              activeTab,
                                                              onTabChange,
                                                              showHeader = true,
                                                          }) => {
    const auth = useAuth();
    useCreateUser(); // авто-синхронизация
    const userFullName = useUserFullName();

    const handleLogout = () => auth.signoutRedirect();

    const authInfo = {
        isAuthenticated: auth.isAuthenticated,
        userFullName,
        onLogin: () => {
        },
        onLogout: handleLogout,
    };

    return (
        <>
            {showHeader && auth.isAuthenticated && activeTab && (
                <Header
                    activeTab={activeTab as any}
                    onTabChange={onTabChange as any}
                    authInfo={authInfo}
                />
            )}
            {children}
        </>
    );
};