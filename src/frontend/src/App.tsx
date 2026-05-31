import React from 'react';
import {useAuth} from 'react-oidc-context';
import {AuthProvider} from './providers/AuthProvider';
import {NotificationProvider} from './context/NotificationContext';
import {NotificationToastContainer} from './components/NotificationBell';
import LoginGate from './components/auth/LoginGate';
import StartDashboardCards from './components/StartDashboardCards';
import DashboardTasks from './components/DashboardTasks';
import { Navigate, Route, Routes } from 'react-router-dom';
import './App.css';

function App() {
    const auth = useAuth();

    // Показываем загрузку аутентификации
    if (auth.isLoading) return <div className="auth-loading">Загрузка аутентификации...</div>;

    // Показываем ошибку аутентификации
    if (auth.error) return <div className="auth-error">Ошибка авторизации: {auth.error.message}</div>;

    // Контент для аутентифицированных пользователей
    const content = auth.isAuthenticated ? (
        <NotificationProvider>
            <AuthProvider>
                <Routes>
                    <Route path="/" element={<StartDashboardCards showHeader={auth.isAuthenticated} />} />
                    <Route path="/projects/:projectId" element={<DashboardTasks showHeader={auth.isAuthenticated} />} />
                    <Route path="*" element={<Navigate to="/" replace />} />
                 </Routes>
                <NotificationToastContainer />
            </AuthProvider>
        </NotificationProvider>
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
