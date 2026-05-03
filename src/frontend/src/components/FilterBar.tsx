import React from 'react';
import { FaFilter, FaTimes, FaCog } from 'react-icons/fa';
import { useTasks } from '../context/TaskContext';
import { fetchUsers } from '../api/taskService';
import { User } from '../types';
import { fetchIssuePriorities } from '../api/collectionService';
import { IssuePriorityResponse } from '../types/issue';
import ProjectSettingsModal from './ProjectSettingsModal';
import './FilterBar.css';

interface FilterBarProps {
  projectId: string;
}

function FilterBar({ projectId }: FilterBarProps) {
  const { filters, setFilter, resetFilters } = useTasks();
  const [users, setUsers] = React.useState<User[]>([]);
  const [priorities, setPriorities] = React.useState<IssuePriorityResponse[]>([]);
  const [isSettingsOpen, setIsSettingsOpen] = React.useState(false);

  // Загрузка пользователей
  React.useEffect(() => {
    fetchUsers().then(setUsers);
  }, []);

  React.useEffect(() => {
    fetchIssuePriorities()
      .then(setPriorities)
      .catch((error) => {
        console.error('Ошибка загрузки приоритетов:', error);
        setPriorities([]);
      });
  }, []);

  // Проверка, есть ли активные фильтры
  const hasActiveFilters = Object.values(filters).some(value => value !== '');

  const handleFilterChange = (key: keyof typeof filters, value: string) => {
    setFilter(key, value);
  };

  return (
    <div className="filter-bar">
      <div className="filter-bar-content">
        <div className="filter-icon">
          <FaFilter />
        </div>
        <div className="filters">
          <div className="filter-group">
            <label htmlFor="filter-assigned">Исполнитель</label>
            <select
              id="filter-assigned"
              value={filters.assignedTo}
              onChange={(e) => handleFilterChange('assignedTo', e.target.value)}
            >
              <option value="">Все</option>
              {users.map(user => (
                <option key={user.id} value={user.id}>
                  {user.username || user.email || user.id}
                </option>
              ))}
            </select>
          </div>

          <div className="filter-group">
            <label htmlFor="filter-type">Тип задачи</label>
            <select
              id="filter-type"
              value={filters.type}
              onChange={(e) => handleFilterChange('type', e.target.value)}
            >
              <option value="">Все</option>
              <option value="bug">Ошибка</option>
              <option value="feature">Фича</option>
              <option value="improvement">Улучшение</option>
            </select>
          </div>

          <div className="filter-group">
            <label htmlFor="filter-priority">Приоритет</label>
            <select
              id="filter-priority"
              value={filters.priority}
              onChange={(e) => handleFilterChange('priority', e.target.value)}
            >
              <option value="">Все</option>
              {priorities.map((priority) => (
                <option key={priority.number} value={priority.name}>
                  {priority.displayName}
                </option>
              ))}
            </select>
          </div>

          <div className="filter-group">
            <label htmlFor="filter-created">Дата создания</label>
            <select
              id="filter-created"
              value={filters.createdAt}
              onChange={(e) => handleFilterChange('createdAt', e.target.value)}
            >
              <option value="">Все</option>
              <option value="today">За сегодня</option>
              <option value="week">За неделю</option>
              <option value="month">За месяц</option>
              <option value="all">Все</option>
            </select>
          </div>

          <div className="filter-group">
            <label htmlFor="filter-deadline">Дедлайн</label>
            <select
              id="filter-deadline"
              value={filters.deadline}
              onChange={(e) => handleFilterChange('deadline', e.target.value)}
            >
              <option value="">Все</option>
              <option value="overdue">Просроченные</option>
              <option value="this-week">На этой неделе</option>
              <option value="this-month">В этом месяце</option>
              <option value="no-deadline">Без дедлайна</option>
            </select>
          </div>
        </div>

        {hasActiveFilters && (
          <button className="reset-filters-btn" onClick={resetFilters}>
            <FaTimes />
            Сбросить фильтры
          </button>
        )}

        <button className="settings-btn" onClick={() => setIsSettingsOpen(true)} title="Настройки проекта">
          <FaCog />
        </button>
      </div>

      <ProjectSettingsModal
        open={isSettingsOpen}
        onOpenChange={setIsSettingsOpen}
        projectId={projectId}
      />
    </div>
  );
}

export default FilterBar;
