import React from 'react';
import {FaBug, FaCircle, FaLightbulb, FaRocket} from 'react-icons/fa';
import {TaskCardProps} from '../types';
import {formatDateOnlyRu} from '../utils/dateOnly';
import './TaskCard.css';

function TaskCard({task, onClick}: TaskCardProps) {
    // Иконки для типов задач
    const getTypeIcon = (type: string) => {
        switch (type) {
            case 'bug':
                return <FaBug className="task-type-icon bug"/>;
            case 'feature':
                return <FaRocket className="task-type-icon feature"/>;
            case 'improvement':
                return <FaLightbulb className="task-type-icon improvement"/>;
            default:
                return <FaCircle className="task-type-icon"/>;
        }
    };

    // Проверка просроченности дедлайна
    const isOverdue = task.dueDate && new Date(task.dueDate) < new Date();

    // Определяем класс рамки по приоритету
    let priorityBorderClass = '';
    switch (task.issuePriority.name) {
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
                    {getTypeIcon(task.issueType.name)}
                    <h3 className="task-title">{task.name}</h3>
                </div>
                <div className="task-priority">
                </div>
            </div>

            {task.description && (
                <p className="task-description">{task.description}</p>
            )}

            <div className="task-footer">
                {task.dueDate && (
                    <div className={`task-deadline ${isOverdue ? 'overdue' : ''}`}>
                        {formatDateOnlyRu(task.dueDate)}
                    </div>
                )}
            </div>
        </div>
    );
}

export default TaskCard;