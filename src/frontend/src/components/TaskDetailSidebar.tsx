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
import { TaskDetailSidebarProps, Task, User } from '../types';
import './TaskDetailSidebar.css';

function TaskDetailSidebar() {
  const { selectedTask, setSelectedTask, updateTask, deleteTask } = useTasks();
  const [users, setUsers] = useState<User[]>([]);
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    issueStatusId: '1',
    numberIssueType: 2,
    numberIssuePriority: 2,
    dueDate: ''
  });

  // Загрузка пользователей
  useEffect(() => {
    fetchUsers().then(setUsers);
  }, []);

  // Обновление формы при изменении выбранной задачи
  useEffect(() => {
    if (selectedTask) {
      setFormData({
        name: selectedTask.name || '',
        description: selectedTask.description || '',
        issueStatusId: selectedTask.taskStatusId,
        numberIssueType: selectedTask.issueType.number,
        numberIssuePriority: selectedTask.issuePriority.number,
        dueDate: selectedTask.dueDate
          ? selectedTask.dueDate.split('T')[0]
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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSave = async () => {
    try {
      const updates = {
        name: formData.name,
        description: formData.description,
        issueStatusId: formData.issueStatusId,
        numberIssueType: parseInt(formData.numberIssueType.toString()),
        numberIssuePriority: parseInt(formData.numberIssuePriority.toString()),
        dueDate: formData.dueDate ? formData.dueDate : null
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

  const getTypeIcon = (type: string) => {
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

  const getPriorityIcon = (priority: string) => {
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
                  name="name"
                  value={formData.name}
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
                  rows={4}
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
                  <h3>{selectedTask.name}</h3>
                  <div className="detail-icons">
                    {getTypeIcon(selectedTask.issueType.name)}
                    {getPriorityIcon(selectedTask.issuePriority.name)}
                  </div>
                </div>
              </div>

              <div className="detail-section">
                <label>Описание</label>
                <p className="detail-text">{selectedTask.description || 'Нет описания'}</p>
              </div>

              <div className="detail-section">
                <label>Тип задачи</label>
                <p className="detail-text">
                  {selectedTask.issueType.displayName}
                </p>
              </div>

              <div className="detail-section">
                <label>Важность</label>
                <p className="detail-text">
                  {selectedTask.issuePriority.displayName}
                </p>
              </div>

              <div className="detail-section">
                <label>Исполнитель</label>
                <p className="detail-text">Не назначен</p>
              </div>

              {selectedTask.dueDate && (
                <div className="detail-section">
                  <label>Дедлайн</label>
                  <p className="detail-text">
                    {new Date(selectedTask.dueDate).toLocaleDateString('ru-RU', {
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
                  {new Date(selectedTask.createdDate).toLocaleDateString('ru-RU', {
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