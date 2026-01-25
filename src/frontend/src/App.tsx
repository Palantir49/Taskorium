import React, {useState} from 'react';
import {useAuth} from 'react-oidc-context';
import {TaskProvider} from './context/TaskContext';
import {AuthProvider} from './providers/AuthProvider';
import LoginGate from './components/auth/LoginGate';
import FilterBar from './components/FilterBar';
import KanbanBoard from './components/KanbanBoard';
import TaskDetailSidebar from './components/TaskDetailSidebar';
import TaskCreateForm from './components/TaskCreateForm';
import {TaskStatus} from './types';
import './App.css';

type TabType = 'board' | 'analytics' | 'docs';

function App() {
    const [activeTab, setActiveTab] = useState<TabType>('board');
    const [showCreateForm, setShowCreateForm] = useState(false);
    const [createFormStatus, setCreateFormStatus] = useState<TaskStatus>('backlog');

    const auth = useAuth();

    const handleOpenCreateForm = (status: TaskStatus) => {
        setCreateFormStatus(status);
        setShowCreateForm(true);
    };

    const handleCloseCreateForm = () => {
        setShowCreateForm(false);
    };

    if (auth.isLoading) return <div>Loading...</div>;
    if (auth.error) return <div>Error: {auth.error.message}</div>;

    const content = auth.isAuthenticated ? (
        <>
            {activeTab === 'board' && (
                <>
                    <FilterBar/>
                    <KanbanBoard onCreateTask={handleOpenCreateForm}/>
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
            <TaskCreateForm
                isOpen={showCreateForm}
                onClose={handleCloseCreateForm}
                initialStatus={createFormStatus}
            />
        </>
    ) : (
        <LoginGate/>
    );

    return (
        <TaskProvider>
            <div className="app">
                <AuthProvider
                    activeTab={activeTab}
                    onTabChange={setActiveTab}
                    showHeader={auth.isAuthenticated}
                >
                    {content}
                </AuthProvider>
            </div>
        </TaskProvider>
    );
}

export default App;
