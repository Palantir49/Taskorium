import React from 'react';
import {TaskProvider} from '../context/TaskContext';
import {useAuthContext} from '../providers/AuthProvider';
import FilterBar from './FilterBar';
import KanbanBoard from './KanbanBoard';
import TaskDetailSidebar from './TaskDetailSidebar';
import TaskCreateForm from './TaskCreateForm';
import HeaderKanbanBoard from './HeaderKanbanBoard';
import {TaskStatus} from '../types';
import {useParams, useSearchParams} from 'react-router-dom';
import './DashboardTasks.css';

interface DashboardTasksProps {
    showHeader?: boolean;
}

function DashboardTasks({showHeader = true}: DashboardTasksProps) {
    const authInfo = useAuthContext();
    const {projectId = ''} = useParams<{ projectId: string }>();
    const [searchParams, setSearchParams] = useSearchParams();
    const activeTab = searchParams.get('tab') ?? 'board';

    const handleTabChange = (tab: string) => {
        const next = new URLSearchParams(searchParams);
        next.set('tab', tab);
        setSearchParams(next);
    };

    const [showCreateForm, setShowCreateForm] = React.useState(false);
    const [createFormStatus, setCreateFormStatus] = React.useState<TaskStatus>('backlog');

    const handleOpenCreateForm = (status: TaskStatus) => {
        setCreateFormStatus(status);
        setShowCreateForm(true);
    };

    const handleCloseCreateForm = () => {
        setShowCreateForm(false);
    };

    const content = (
        <>
            {activeTab === 'board' && (
                <>
                    <FilterBar projectId={projectId} onCreateTask={() => handleOpenCreateForm('backlog')}/>
                    <KanbanBoard projectId={projectId}/>
                </>
            )}
            {activeTab === 'analytics' && (
                <div className="analytics-frame-wrapper">
                    <iframe
                        src="https://grafana.taskorium.ru/public-dashboards/81e74df4709b4523b7dfe184de5ae19d"
                        title="Аналитика"
                        className="analytics-frame"
                        allowFullScreen
                    />
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
                projectId={projectId}
                initialStatus={createFormStatus}
            />
        </>
    );

    return (
        <TaskProvider projectId={projectId}>
            <div className="app">
                {showHeader && authInfo.isAuthenticated && (
                    <HeaderKanbanBoard
                        activeTab={activeTab}
                        onTabChange={handleTabChange}
                        authInfo={authInfo}
                    />
                )}
                {content}
            </div>
        </TaskProvider>
    );
}

export default DashboardTasks;
