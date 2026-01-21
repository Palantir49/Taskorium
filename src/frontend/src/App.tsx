import React, {useState} from 'react';
import {TaskProvider} from './context/TaskContext';
import Header from './components/Header';
import FilterBar from './components/FilterBar';
import KanbanBoard from './components/KanbanBoard';
import TaskDetailSidebar from './components/TaskDetailSidebar';
import TaskCreateForm from './components/TaskCreateForm';
import { TaskStatus } from './types';
import './App.css';
import {hasAuthParams, useAuth} from "react-oidc-context";

type TabType = 'board' | 'analytics' | 'docs';

function App() {
  const [activeTab, setActiveTab] = useState<TabType>('board');
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [createFormStatus, setCreateFormStatus] = useState<TaskStatus>('backlog');

  const handleOpenCreateForm = (status: TaskStatus) => {
    setCreateFormStatus(status);
    setShowCreateForm(true);
  };

  const handleCloseCreateForm = () => {
    setShowCreateForm(false);
  };
    const [activeTab, setActiveTab] = useState('board');
    const auth = useAuth();
    const hasTriedSignin = React.useRef(false);

    // Обработчик логина
    const handleLogin = () => {
        auth.signinRedirect();
        hasTriedSignin.current = true;
    };

    // Обработчик логаута
    const handleLogout = () => {
        auth.signoutRedirect();
    };

    // automatically sign-in
    React.useEffect(() => {
        if (!hasAuthParams() &&
            !auth.isAuthenticated && !auth.activeNavigator && !auth.isLoading &&
            !hasTriedSignin.current
        ) {
            auth.signinRedirect();
            hasTriedSignin.current = true;
        }
    }, [auth]);

    // Получаем полное имя пользователя
    const getUserFullName = () => {
        if (auth.isAuthenticated && auth.user) {
            const {profile} = auth.user;
            // Предполагаем, что имя и фамилия хранятся в стандартных полях OIDC
            const firstName = profile.given_name || profile.name || '';
            const lastName = profile.family_name || '';

            // Если оба поля есть - объединяем, иначе используем то, что есть
            if (firstName && lastName) {
                return `${firstName} ${lastName}`;
            } else if (profile.name) {
                return profile.name;
            } else if (profile.preferred_username) {
                return profile.preferred_username;
            } else {
                return 'Пользователь';
            }
        }
        return '';
    };

    if (auth.isLoading) return <div>Loading...</div>;
    if (auth.error) return <div>Error: {auth.error.message}</div>;

    return (
        <TaskProvider>
            <div className="app">
                {/* Добавляем блок аутентификации в Header */}
                <Header
                    activeTab={activeTab}
                    onTabChange={setActiveTab}
                    authInfo={{
                        isAuthenticated: auth.isAuthenticated,
                        userFullName: getUserFullName(),
                        onLogin: handleLogin,
                        onLogout: handleLogout
                    }}
                />

                {/* Основной контент показываем только для аутентифицированных пользователей */}
                {auth.isAuthenticated ? (
                    <>
                        {activeTab === 'board' && (
                            <>
                                <FilterBar/>
                                <KanbanBoard/>
                            </>
                        )}
                        {activeTab === 'analytics' && (
                            <div className="coming-soon">
                                <h2>Аналитика</h2>
                                <p>Раздел находится в разработке</p>
                            </div>
                        )}
                        {activeTab === 'docs' && (
                            <div className="coming-soon">
                                <h2>Документация</h2>
                                <p>Раздел находится в разработке</p>
                            </div>
                        )}
                        <TaskDetailSidebar/>
                    </>
                ) : (
                    // Отображаем приветственный экран для неаутентифицированных пользователей
                    <div className="login-container">
                        <div className="login-content">
                            <h2>Добро пожаловать в Kanban Board</h2>
                            <p>Для работы с приложением необходимо войти в систему</p>
                            <button
                                onClick={handleLogin}
                                className="login-button"
                            >
                                Войти
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </TaskProvider>
    );
  return (
    <TaskProvider>
      <div className="app">
        <Header activeTab={activeTab} onTabChange={setActiveTab} />
        {activeTab === 'board' && (
          <>
            <FilterBar />
            <KanbanBoard onCreateTask={handleOpenCreateForm} />
          </>
        )}
        {activeTab === 'analytics' && (
          <div className="coming-soon">
            <h2>Аналитика</h2>
            <p>Раздел находится в разработке</p>
          </div>
        )}
        {activeTab === 'docs' && (
          <div className="coming-soon">
            <h2>Документация</h2>
            <p>Раздел находится в разработке</p>
          </div>
        )}
        <TaskDetailSidebar />
        <TaskCreateForm
          isOpen={showCreateForm}
          onClose={handleCloseCreateForm}
          initialStatus={createFormStatus}
        />
      </div>
    </TaskProvider>
  );
}

export default App;


