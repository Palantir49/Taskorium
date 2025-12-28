import React from 'react';
import { FaFilter, FaTimes } from 'react-icons/fa';
import { useTasks } from '../context/TaskContext';
import { fetchUsers } from '../api/taskService';
import './FilterBar.css';

function FilterBar() {
  const { filters, setFilter, resetFilters } = useTasks();
  const [users, setUsers] = React.useState([]);

  // Загрузка пользователей
  React.useEffect(() => {
    fetchUsers().then(setUsers);
  }, []);

  // Проверка, есть ли активные фильтры
  const hasActiveFilters = Object.values(filters).some(value => value !== '');

  const handleFilterChange = (key, value) => {
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
                  {user.name}
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
            <label htmlFor="filter-priority">Важность</label>
            <select
              id="filter-priority"
              value={filters.priority}
              onChange={(e) => handleFilterChange('priority', e.target.value)}
            >
              <option value="">Все</option>
              <option value="critical">Критичная</option>
              <option value="high">Высокая</option>
              <option value="medium">Средняя</option>
              <option value="low">Низкая</option>
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
      </div>
    </div>
  );
}

export default FilterBar;


