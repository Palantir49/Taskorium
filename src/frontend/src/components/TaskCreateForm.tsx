import React, { useState, useEffect } from 'react';
import {
  FaTimes,
  FaSave
} from 'react-icons/fa';
import { useTasks } from '../context/TaskContext';
import { fetchUsers } from '../api/taskService';
import { TaskCreateFormProps, User } from '../types';
import './TaskDetailSidebar.css';

function TaskCreateForm({ isOpen, onClose, initialStatus = 'backlog' }: TaskCreateFormProps) {
  const { createTask } = useTasks();
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    projectId: '1', // По умолчанию проект 1
    numberIssueType: 2, // По умолчанию функция
    numberIssuePriority: 2, // По умолчанию средний приоритет
    dueDate: ''
  });

  // Загрузка пользователей
  useEffect(() => {
    if (isOpen) {
      fetchUsers().then(setUsers);
      // Сброс формы при открытии
      setFormData({
        name: '',
        description: '',
        projectId: '1',
        numberIssueType: 2,
        numberIssuePriority: 2,
        dueDate: ''
      });
    }
  }, [isOpen, initialStatus]);

  if (!isOpen) return null;

  const handleClose = () => {
    onClose();
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSave = async () => {
    if (!formData.name.trim()) {
      alert('Название задачи обязательно для заполнения');
      return;
    }

    setIsLoading(true);
    try {
      const taskData = {
        name: formData.name.trim(),
        description: formData.description.trim(),
        projectId: formData.projectId,
        numberIssueType: parseInt(formData.numberIssueType.toString()),
        numberIssuePriority: parseInt(formData.numberIssuePriority.toString()),
        dueDate: formData.dueDate ? formData.dueDate : null
      };

      await createTask(taskData);
      onClose();
    } catch (error) {
      console.error('Ошибка при создании задачи:', error);
      alert('Не удалось создать задачу');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="task-detail-sidebar-overlay" onClick={handleClose}>
      <div className="task-detail-sidebar" onClick={(e) => e.stopPropagation()}>
        <div className="sidebar-header">
          <h2>Создать задачу</h2>
          <button className="close-btn" onClick={handleClose}>
            <FaTimes />
          </button>
        </div>

        <div className="sidebar-content">
          <div className="form-group">
            <label>Название *</label>
            <input
              type="text"
              name="name"
              value={formData.name}
              onChange={handleChange}
              className="form-input"
              placeholder="Введите название задачи"
              required
            />
          </div>

          <div className="form-group">
            <label>Описание</label>
            <textarea
              name="description"
              value={formData.description}
              onChange={handleChange}
              className="form-textarea"
              rows={4}
              placeholder="Опишите задачу"
            />
          </div>

          <div className="form-group">
            <label>Тип задачи</label>
            <select
              name="numberIssueType"
              value={formData.numberIssueType}
              onChange={handleChange}
              className="form-select"
            >
              <option value="1">Ошибка</option>
              <option value="2">Фича</option>
              <option value="3">Улучшение</option>
            </select>
          </div>

          <div className="form-group">
            <label>Важность</label>
            <select
              name="numberIssuePriority"
              value={formData.numberIssuePriority}
              onChange={handleChange}
              className="form-select"
            >
              <option value="4">Критичная</option>
              <option value="3">Высокая</option>
              <option value="2">Средняя</option>
              <option value="1">Низкая</option>
            </select>
          </div>

          <div className="form-group">
            <label>Дедлайн</label>
            <input
              type="date"
              name="dueDate"
              value={formData.dueDate}
              onChange={handleChange}
              className="form-input"
            />
          </div>

          <div className="form-actions">
            <button
              className="btn-save"
              onClick={handleSave}
              disabled={isLoading}
            >
              <FaSave />
              {isLoading ? 'Создание...' : 'Создать'}
            </button>
            <button
              className="btn-cancel"
              onClick={handleClose}
              disabled={isLoading}
            >
              Отмена
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default TaskCreateForm;