import React, { useState } from 'react';
import { TaskProvider } from './context/TaskContext';
import Header from './components/Header';
import FilterBar from './components/FilterBar';
import KanbanBoard from './components/KanbanBoard';
import TaskDetailSidebar from './components/TaskDetailSidebar';
import './App.css';

function App() {
  const [activeTab, setActiveTab] = useState('board');

  return (
    <TaskProvider>
      <div className="app">
        <Header activeTab={activeTab} onTabChange={setActiveTab} />
        {activeTab === 'board' && (
          <>
            <FilterBar />
            <KanbanBoard />
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
      </div>
    </TaskProvider>
  );
}

export default App;


