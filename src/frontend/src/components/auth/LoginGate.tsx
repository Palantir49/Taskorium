// src/components/auth/LoginGate.tsx
import React from 'react';
import {useAuth} from 'react-oidc-context';

const LoginGate: React.FC = () => {
    const auth = useAuth();
    const lang = navigator.language.toLowerCase();
    const isRu = lang.startsWith('ru');
    const loginText = isRu ? 'Продолжить с SSO' : 'Continue with SSO';

    const handleLogin = () => {
        auth.signinRedirect();
    };

    if (auth.isLoading) return <div>Loading...</div>;
    if (auth.error) return <div>Error: {auth.error.message}</div>;

    return (
        <div className="login-container">
            <div className="login-content">
                <h2>{isRu ? 'Добро пожаловать в систему управления задачами и проектами Taskorium!' : 'Welcome to the Taskorium task and project management system!'}</h2>
                <p>{isRu ? 'Для работы необходимо войти' : 'Please sign in to continue'}</p>
                <button onClick={handleLogin} className="login-button">
                    {loginText}
                </button>
            </div>
        </div>
    );
};

export default LoginGate;
