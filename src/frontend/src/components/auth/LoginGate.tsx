// src/components/auth/LoginGate.tsx
import React from 'react';
import {useAuth} from 'react-oidc-context';
import './LoginGate.css'

const LoginGate: React.FC = () => {
    const auth = useAuth();

    if (auth.isLoading) return <div className="login-container"><p>Загрузка...</p></div>;
    if (auth.error) return <div className="login-container"><p>Ошибка: {auth.error.message}</p></div>;

    return (
        <div className="login-container">
            <div className="login-card">
                <div className="login-logo">
                    <div className="login-logo-icon">🗂</div>
                    <span className="login-logo-text">Taskorium</span>
                </div>

                <div className="login-divider"/>

                <div style={{textAlign: 'center'}}>
                    <p className="login-title">Добро пожаловать</p>
                    <p className="login-subtitle">
                        Сервис управления задачами и проектами.<br/>
                        Для работы необходимо войти.
                    </p>
                </div>

                <button onClick={() => auth.signinRedirect()} className="login-button">
                    🔑 Войти с помощью Keycloak
                </button>

                <p className="login-footer">Единый вход через корпоративный аккаунт</p>
            </div>
        </div>
    );
};

export default LoginGate;
