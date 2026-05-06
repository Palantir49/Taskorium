import React, { useState } from 'react';
import {
  FaTimes,
  FaBug,
  FaRocket,
  FaLightbulb,
  FaFire,
  FaExclamationTriangle,
  FaInfoCircle,
  FaCircle,
  FaTrash
} from 'react-icons/fa';
import { useTasks } from '../context/TaskContext';
import { Task } from '../types';
import TaskCreateForm from './TaskCreateForm';
import './TaskDetailSidebar.css';

function TaskDetailSidebar() {
  const { selectedTask, setSelectedTask, deleteTask } = useTasks();
  const [isEditFormOpen, setIsEditFormOpen] = useState(false);
  const [taskForEdit, setTaskForEdit] = useState<Task | null>(null);

  if (!selectedTask) return null;

  const handleClose = () => {
    setSelectedTask(null);
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

  const handleOpenEditForm = () => {
    setTaskForEdit(selectedTask);
    setIsEditFormOpen(true);
  };

  const handleCloseEditForm = () => {
    setIsEditFormOpen(false);
    setTaskForEdit(null);
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
                <button className="btn-edit" onClick={handleOpenEditForm}>
                  Редактировать
                </button>
                <button className="btn-delete" onClick={handleDelete}>
                  <FaTrash />
                  Удалить
                </button>
              </div>
          </>
        </div>
      </div>

      <TaskCreateForm
        isOpen={isEditFormOpen}
        onClose={handleCloseEditForm}
        projectId={selectedTask.projectId}
        mode="edit"
        task={taskForEdit}
        onSaved={handleClose}
      />
    </div>
  );
}

export default TaskDetailSidebar;