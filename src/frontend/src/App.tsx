import React, { useState } from 'react';
import { TaskProvider } from './context/TaskContext';
import Header from './components/Header';
import FilterBar from './components/FilterBar';
import KanbanBoard from './components/KanbanBoard';
import TaskDetailSidebar from './components/TaskDetailSidebar';
import TaskCreateForm from './components/TaskCreateForm';
import { TaskStatus } from './types';
import './App.css';

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
