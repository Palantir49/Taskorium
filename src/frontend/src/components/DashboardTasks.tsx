import React from 'react';
import {TaskProvider} from '../context/TaskContext';
import {useAuthContext} from '../providers/AuthProvider';
import FilterBar from './FilterBar';
import KanbanBoard from './KanbanBoard';
import TaskDetailSidebar from './TaskDetailSidebar';
import TaskCreateForm from './TaskCreateForm';
import HeaderKanbanBoard from './HeaderKanbanBoard';
import {TaskStatus} from '../types';

interface DashboardTasksProps {
  activeTab: string;
  onTabChange: React.Dispatch<React.SetStateAction<string>>;
  showHeader?: boolean;
}

function DashboardTasks({ activeTab, onTabChange, showHeader = true }: DashboardTasksProps) {
  const authInfo = useAuthContext();
  
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
    </>
  );

  return (
    <TaskProvider>
      <div className="app">
        {showHeader && authInfo.isAuthenticated && (
          <HeaderKanbanBoard
            activeTab={activeTab}
            onTabChange={onTabChange}
            authInfo={authInfo}
          />
        )}
        {content}
      </div>
    </TaskProvider>
  );
}

export default DashboardTasks;
