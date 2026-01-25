import React from 'react';
import {
  FaBug,
  FaRocket,
  FaLightbulb,
  FaFire,
  FaExclamationTriangle,
  FaInfoCircle,
  FaCircle
} from 'react-icons/fa';
import { TaskCardProps, TaskType } from '../types';
import './TaskCard.css';

function TaskCard({ task, onClick }: TaskCardProps) {
  // Иконки для типов задач
  const getTypeIcon = (type: TaskType) => {
    switch (type) {
      case 'bug':
        return <FaBug className="task-type-icon bug" />;
      case 'feature':
        return <FaRocket className="task-type-icon feature" />;
      case 'improvement':
        return <FaLightbulb className="task-type-icon improvement" />;
      default:
        return <FaCircle className="task-type-icon" />;
    }
  };

  // Форматирование даты
  const formatDate = (date: Date | string | null): string | null => {
    if (!date) return null;
    const d = new Date(date);
    return d.toLocaleDateString('ru-RU', { day: 'numeric', month: 'short' });
  };

  // Проверка просроченности дедлайна
  const isOverdue = task.deadline && new Date(task.deadline) < new Date();

  // Определяем класс рамки по приоритету
  let priorityBorderClass = '';
  switch (task.priority) {
    case 'critical':
      priorityBorderClass = 'priority-critical';
      break;
    case 'high':
      priorityBorderClass = 'priority-high';
      break;
    case 'medium':
      priorityBorderClass = 'priority-medium';
      break;
    case 'low':
      priorityBorderClass = 'priority-low';
      break;
    default:
      break;
  }

  return (
    <div className={`task-card ${priorityBorderClass}`} onClick={() => onClick && onClick(task)}>
      <div className="task-card-header">
        <div className="task-title-row">
          {getTypeIcon(task.type)}
          <h3 className="task-title">{task.title}</h3>
        </div>
        <div className="task-priority">
        </div>
      </div>

      {task.description && (
        <p className="task-description">{task.description}</p>
      )}

      <div className="task-footer">
        <div className="task-assignee">
          {task.assignedTo && (
            <div className="assignee-avatar" title={task.assignedTo.name}>
              {task.assignedTo.initials}
            </div>
          )}
        </div>
        {task.deadline && (
          <div className={`task-deadline ${isOverdue ? 'overdue' : ''}`}>
            {formatDate(task.deadline)}
          </div>
        )}
      </div>
    </div>
  );
}

export default TaskCard;
