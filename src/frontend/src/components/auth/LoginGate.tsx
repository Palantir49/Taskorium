// src/components/auth/LoginGate.tsx
import React from 'react';
import {useAuth} from 'react-oidc-context';

const LoginGate: React.FC = () => {
    const auth = useAuth();
    const lang = navigator.language.toLowerCase();
    const isRu = lang.startsWith('ru');
    const loginText = 'Войти с помощью Keycloak';

    const handleLogin = () => {
        auth.signinRedirect();
    };

    if (auth.isLoading) return <div>Loading...</div>;
    if (auth.error) return <div>Error: {auth.error.message}</div>;

    return (
        <div className="login-container">
            <div className="login-content">
                <h2>Добро пожаловать в сервис управления задачами и проектами Taskorium!</h2>
                <p>Для работы необходимо войти</p>
                <button onClick={handleLogin} className="login-button">
                    {loginText}
                </button>
            </div>
        </div>
    );
};

export default LoginGate;
