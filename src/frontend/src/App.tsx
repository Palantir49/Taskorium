import React, {useState} from 'react';
import {useAuth} from 'react-oidc-context';
import {AuthProvider} from './providers/AuthProvider';
import LoginGate from './components/auth/LoginGate';
import StartDashboardCards from './components/StartDashboardCards';
import './App.css';

type TabType = 'board' | 'analytics' | 'docs';

function App() {
    const [activeTab, setActiveTab] = useState<string>('board');

    const auth = useAuth();

    // Показываем загрузку аутентификации
    if (auth.isLoading) return <div className="auth-loading">Загрузка аутентификации...</div>;

    // Показываем ошибку аутентификации
    if (auth.error) return <div className="auth-error">Ошибка авторизации: {auth.error.message}</div>;

    // Контент для аутентифицированных пользователей
    const content =  auth.isAuthenticated ? (
        <AuthProvider>
            <StartDashboardCards
                activeTab={activeTab}
                onTabChange={setActiveTab}
                showHeader={auth.isAuthenticated}
            />
        </AuthProvider>
    ) : (
        <LoginGate/>
    );

    return (
        <div className="app">
            {content}
        </div>
    );
}

export default App;
