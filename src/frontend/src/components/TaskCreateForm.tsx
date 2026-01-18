import React, { useState, useEffect } from 'react';
import {
  FaTimes,
  FaSave
} from 'react-icons/fa';
import { useTasks } from '../context/TaskContext';
import { fetchUsers } from '../api/taskService';
import { TaskCreateFormProps, User, TaskStatus, TaskPriority, TaskType } from '../types';
import './TaskDetailSidebar.css';

function TaskCreateForm({ isOpen, onClose, initialStatus = 'backlog' }: TaskCreateFormProps) {
  console.log('TaskCreateForm render - isOpen:', isOpen, 'initialStatus:', initialStatus);
  const { createTask } = useTasks();
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    status: initialStatus,
    priority: 'medium' as TaskPriority,
    type: 'feature' as TaskType,
    assignedTo: '',
    deadline: ''
  });

  // Загрузка пользователей
  useEffect(() => {
    if (isOpen) {
      fetchUsers().then(setUsers);
      // Сброс формы при открытии
      setFormData({
        title: '',
        description: '',
        status: initialStatus,
        priority: 'medium',
        type: 'feature',
        assignedTo: '',
        deadline: ''
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
    if (!formData.title.trim()) {
      alert('Название задачи обязательно для заполнения');
      return;
    }

    setIsLoading(true);
    try {
      const taskData = {
        title: formData.title.trim(),
        description: formData.description.trim(),
        status: formData.status,
        priority: formData.priority,
        type: formData.type,
        assignedTo: formData.assignedTo ? users.find(u => u.id.toString() === formData.assignedTo) : undefined,
        deadline: formData.deadline ? new Date(formData.deadline) : null
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
              name="title"
              value={formData.title}
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
            <label>Статус</label>
            <select
              name="status"
              value={formData.status}
              onChange={handleChange}
              className="form-select"
            >
              <option value="backlog">Бэклог</option>
              <option value="in-progress">В работе</option>
              <option value="testing">В тестировании</option>
              <option value="pause">Пауза</option>
              <option value="done">Готово</option>
            </select>
          </div>

          <div className="form-group">
            <label>Тип задачи</label>
            <select
              name="type"
              value={formData.type}
              onChange={handleChange}
              className="form-select"
            >
              <option value="bug">Ошибка</option>
              <option value="feature">Фича</option>
              <option value="improvement">Улучшение</option>
            </select>
          </div>

          <div className="form-group">
            <label>Важность</label>
            <select
              name="priority"
              value={formData.priority}
              onChange={handleChange}
              className="form-select"
            >
              <option value="critical">Критичная</option>
              <option value="high">Высокая</option>
              <option value="medium">Средняя</option>
              <option value="low">Низкая</option>
            </select>
          </div>

          <div className="form-group">
            <label>Исполнитель</label>
            <select
              name="assignedTo"
              value={formData.assignedTo}
              onChange={handleChange}
              className="form-select"
            >
              <option value="">Не назначен</option>
              {users.map(user => (
                <option key={user.id} value={user.id}>
                  {user.name}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Дедлайн</label>
            <input
              type="date"
              name="deadline"
              value={formData.deadline}
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
