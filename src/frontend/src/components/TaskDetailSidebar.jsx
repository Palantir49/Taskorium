import React, { useState, useEffect } from 'react';
import {
  FaTimes,
  FaBug,
  FaRocket,
  FaLightbulb,
  FaFire,
  FaExclamationTriangle,
  FaInfoCircle,
  FaCircle,
  FaSave,
  FaTrash
} from 'react-icons/fa';
import { useTasks } from '../context/TaskContext';
import { fetchUsers } from '../api/taskService';
import './TaskDetailSidebar.css';

function TaskDetailSidebar() {
  const { selectedTask, setSelectedTask, updateTask, deleteTask } = useTasks();
  const [users, setUsers] = useState([]);
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    status: '',
    priority: '',
    type: '',
    assignedTo: '',
    deadline: ''
  });

  // Загрузка пользователей
  useEffect(() => {
    fetchUsers().then(setUsers);
  }, []);

  // Обновление формы при изменении выбранной задачи
  useEffect(() => {
    if (selectedTask) {
      setFormData({
        title: selectedTask.title || '',
        description: selectedTask.description || '',
        status: selectedTask.status || '',
        priority: selectedTask.priority || '',
        type: selectedTask.type || '',
        assignedTo: selectedTask.assignedTo?.id?.toString() || '',
        deadline: selectedTask.deadline
          ? new Date(selectedTask.deadline).toISOString().split('T')[0]
          : ''
      });
      setIsEditing(false);
    }
  }, [selectedTask]);

  if (!selectedTask) return null;

  const handleClose = () => {
    setSelectedTask(null);
    setIsEditing(false);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSave = async () => {
    try {
      const updates = {
        title: formData.title,
        description: formData.description,
        status: formData.status,
        priority: formData.priority,
        type: formData.type,
        assignedTo: users.find(u => u.id.toString() === formData.assignedTo),
        deadline: formData.deadline ? new Date(formData.deadline) : null
      };

      await updateTask(selectedTask.id, updates);
      setIsEditing(false);
    } catch (error) {
      console.error('Ошибка при сохранении задачи:', error);
      alert('Не удалось сохранить изменения');
    }
  };

  const handleDelete = async () => {
    if (window.confirm('Вы уверены, что хотите удалить эту задачу?')) {
      try {
        await deleteTask(selectedTask.id);
        handleClose();
      } catch (error) {
        console.error('Ошибка при удалении задачи:', error);
        alert('Не удалось удалить задачу');
      }
    }
  };

  const getTypeIcon = (type) => {
    switch (type) {
      case 'bug':
        return <FaBug className="detail-icon bug" />;
      case 'feature':
        return <FaRocket className="detail-icon feature" />;
      case 'improvement':
        return <FaLightbulb className="detail-icon improvement" />;
      default:
        return <FaCircle className="detail-icon" />;
    }
  };

  const getPriorityIcon = (priority) => {
    switch (priority) {
      case 'critical':
        return <FaFire className="detail-icon critical" />;
      case 'high':
        return <FaExclamationTriangle className="detail-icon high" />;
      case 'medium':
        return <FaInfoCircle className="detail-icon medium" />;
      case 'low':
        return <FaCircle className="detail-icon low" />;
      default:
        return null;
    }
  };

  return (
    <div className="task-detail-sidebar-overlay" onClick={handleClose}>
      <div className="task-detail-sidebar" onClick={(e) => e.stopPropagation()}>
        <div className="sidebar-header">
          <h2>Детали задачи</h2>
          <button className="close-btn" onClick={handleClose}>
            <FaTimes />
          </button>
        </div>

        <div className="sidebar-content">
          {isEditing ? (
            <>
              <div className="form-group">
                <label>Название</label>
                <input
                  type="text"
                  name="title"
                  value={formData.title}
                  onChange={handleChange}
                  className="form-input"
                />
              </div>

              <div className="form-group">
                <label>Описание</label>
                <textarea
                  name="description"
                  value={formData.description}
                  onChange={handleChange}
                  className="form-textarea"
                  rows="4"
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
                <button className="btn-save" onClick={handleSave}>
                  <FaSave />
                  Сохранить
                </button>
                <button className="btn-cancel" onClick={() => setIsEditing(false)}>
                  Отмена
                </button>
              </div>
            </>
          ) : (
            <>
              <div className="detail-section">
                <div className="detail-header">
                  <h3>{selectedTask.title}</h3>
                  <div className="detail-icons">
                    {getTypeIcon(selectedTask.type)}
                    {getPriorityIcon(selectedTask.priority)}
                  </div>
                </div>
              </div>

              <div className="detail-section">
                <label>Описание</label>
                <p className="detail-text">{selectedTask.description || 'Нет описания'}</p>
              </div>

              <div className="detail-section">
                <label>Статус</label>
                <p className="detail-text">
                  {selectedTask.status === 'backlog' && 'Бэклог'}
                  {selectedTask.status === 'in-progress' && 'В работе'}
                  {selectedTask.status === 'testing' && 'В тестировании'}
                  {selectedTask.status === 'pause' && 'Пауза'}
                  {selectedTask.status === 'done' && 'Готово'}
                </p>
              </div>

              <div className="detail-section">
                <label>Тип задачи</label>
                <p className="detail-text">
                  {selectedTask.type === 'bug' && 'Ошибка'}
                  {selectedTask.type === 'feature' && 'Фича'}
                  {selectedTask.type === 'improvement' && 'Улучшение'}
                </p>
              </div>

              <div className="detail-section">
                <label>Важность</label>
                <p className="detail-text">
                  {selectedTask.priority === 'critical' && 'Критичная'}
                  {selectedTask.priority === 'high' && 'Высокая'}
                  {selectedTask.priority === 'medium' && 'Средняя'}
                  {selectedTask.priority === 'low' && 'Низкая'}
                </p>
              </div>

              <div className="detail-section">
                <label>Исполнитель</label>
                {selectedTask.assignedTo ? (
                  <div className="assignee-info">
                    <div className="assignee-avatar-large">
                      {selectedTask.assignedTo.initials}
                    </div>
                    <span>{selectedTask.assignedTo.name}</span>
                  </div>
                ) : (
                  <p className="detail-text">Не назначен</p>
                )}
              </div>

              {selectedTask.deadline && (
                <div className="detail-section">
                  <label>Дедлайн</label>
                  <p className="detail-text">
                    {new Date(selectedTask.deadline).toLocaleDateString('ru-RU', {
                      day: 'numeric',
                      month: 'long',
                      year: 'numeric'
                    })}
                  </p>
                </div>
              )}

              <div className="detail-section">
                <label>Дата создания</label>
                <p className="detail-text">
                  {new Date(selectedTask.createdAt).toLocaleDateString('ru-RU', {
                    day: 'numeric',
                    month: 'long',
                    year: 'numeric'
                  })}
                </p>
              </div>

              <div className="detail-actions">
                <button className="btn-edit" onClick={() => setIsEditing(true)}>
                  Редактировать
                </button>
                <button className="btn-delete" onClick={handleDelete}>
                  <FaTrash />
                  Удалить
                </button>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
}

export default TaskDetailSidebar;


