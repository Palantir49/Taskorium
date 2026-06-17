import React, {useCallback, useState} from 'react';
import {
    FaBug,
    FaCircle,
    FaExclamationTriangle,
    FaFire,
    FaInfoCircle,
    FaLightbulb,
    FaRocket,
    FaTimes,
    FaTrash
} from 'react-icons/fa';
import {useTasks} from '../context/TaskContext';
import {Task} from '../types';
import TaskCreateForm from './TaskCreateForm';
import { downloadAttachment } from '../api/attachmentService';
import { fetchTaskById } from '../api/taskService';
import {formatDateOnlyRu} from '../utils/dateOnly';
import './TaskDetailSidebar.css';

function TaskDetailSidebar() {
    const {selectedTask, setSelectedTask, deleteTask} = useTasks();
    const [isEditFormOpen, setIsEditFormOpen] = useState(false);
    const [taskForEdit, setTaskForEdit] = useState<Task | null>(null);
    const [fullTask, setFullTask] = useState<Task | null>(null);
    const [isLoadingTaskDetails, setIsLoadingTaskDetails] = useState(false);
    const selectedTaskId = selectedTask?.id ?? null;

    const loadTaskDetails = useCallback(async (taskId: string, fallbackTask?: Task | null) => {
        setIsLoadingTaskDetails(true);
        try {
            const task = await fetchTaskById(taskId);
            console.log('TaskDetailSidebar fetched task:', task);
            setFullTask(task);
            return task;
        } catch (error) {
            console.error('Ошибка загрузки полной задачи:', error);
            if (fallbackTask) {
                setFullTask(fallbackTask);
            }
            return fallbackTask ?? null;
        } finally {
            setIsLoadingTaskDetails(false);
        }
    }, []);

    React.useEffect(() => {
        console.log('TaskDetailSidebar selectedTask:', selectedTask);

        if (!selectedTaskId || !selectedTask) {
            setFullTask(null);
            setIsLoadingTaskDetails(false);
            return;
        }

        let cancelled = false;

        setFullTask(selectedTask);
        loadTaskDetails(selectedTaskId, selectedTask).then((task) => {
            if (!cancelled && task) {
                setFullTask(task);
            }
        });

        return () => {
            cancelled = true;
        };
    }, [loadTaskDetails, selectedTask, selectedTaskId]);

    if (!selectedTask) return null;

    const taskDetails = fullTask ?? selectedTask;
    console.log('TaskDetailSidebar taskDetails:', taskDetails);
    console.log('TaskDetailSidebar taskDetails.attachments:', taskDetails.attachments);

    const handleClose = () => {
        setSelectedTask(null);
        setFullTask(null);
    };

    const handleDelete = async () => {
        if (window.confirm('Вы уверены, что хотите удалить эту задачу?')) {
            try {
                await deleteTask(taskDetails.id);
                handleClose();
            } catch (error) {
                console.error('Ошибка при удалении задачи:', error);
                alert('Не удалось удалить задачу');
            }
        }
    };

    const handleOpenEditForm = () => {
        setTaskForEdit(taskDetails);
        setIsEditFormOpen(true);
    };

    const handleDownloadAttachment = async (attachmentId: string, fileName: string) => {
        try {
            const blob = await downloadAttachment(attachmentId);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            link.remove();
            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error('Ошибка скачивания вложения:', error);
            alert('Не удалось скачать файл');
        }
    };

    const handleCloseEditForm = () => {
        setIsEditFormOpen(false);
        setTaskForEdit(null);
    };

    const getTypeIcon = (type: string) => {
        switch (type) {
            case 'bug':
                return <FaBug className="detail-icon bug"/>;
            case 'feature':
                return <FaRocket className="detail-icon feature"/>;
            case 'improvement':
                return <FaLightbulb className="detail-icon improvement"/>;
            default:
                return <FaCircle className="detail-icon"/>;
        }
    };

    const getPriorityIcon = (priority: string) => {
        switch (priority) {
            case 'critical':
                return <FaFire className="detail-icon critical"/>;
            case 'high':
                return <FaExclamationTriangle className="detail-icon high"/>;
            case 'medium':
                return <FaInfoCircle className="detail-icon medium"/>;
            case 'low':
                return <FaCircle className="detail-icon low"/>;
            default:
                return null;
        }
    };
    console.log('assignees:', taskDetails.assignees);
    return (
        <div className="task-detail-sidebar-overlay" onClick={handleClose}>
            <div className="task-detail-sidebar" onClick={(e) => e.stopPropagation()}>
                <div className="sidebar-header">
                    <h2>Детали задачи</h2>
                    <button className="close-btn" onClick={handleClose}>
                        <FaTimes/>
                    </button>
                </div>

                <div className="sidebar-content">
                    {isLoadingTaskDetails && (
                        <div className="detail-section">
                            <p className="detail-text">Загрузка данных задачи...</p>
                        </div>
                    )}
                    <>
                        <div className="detail-section">
                            <div className="detail-header">
                                <h3>{taskDetails.name}</h3>
                                <div className="detail-icons">
                                    {getTypeIcon(taskDetails.issueType.name)}
                                    {getPriorityIcon(taskDetails.issuePriority.name)}
                                </div>
                            </div>
                        </div>

                        <div className="detail-section">
                            <label>Описание</label>
                            <p className="detail-text">{taskDetails.description || 'Нет описания'}</p>
                        </div>

                        <div className="detail-section">
                            <label>Тип задачи</label>
                            <p className="detail-text">
                                {taskDetails.issueType.displayName}
                            </p>
                        </div>

                        <div className="detail-section">
                            <label>Приоритет</label>
                            <p className="detail-text">
                                {taskDetails.issuePriority.displayName}
                            </p>
                        </div>

                        <div className="detail-section">
                            <label>Исполнители</label>
                            <div className="task-assignee">
                                {taskDetails.assignees && taskDetails.assignees.length > 0 ?
                                    taskDetails.assignees.map((assignee) => (
                                        <p key={assignee.userId} className="detail-text">{assignee.userName}</p>

                                    )) : <p className="detail-text">Не назначены</p>
                                }
                            </div>
                        </div>

                        {taskDetails.dueDate && (
                            <div className="detail-section">
                                <label>Дедлайн</label>
                                <p className="detail-text">
                                    {formatDateOnlyRu(taskDetails.dueDate)}
                                </p>
                            </div>
                        )}

                        <div className="detail-section">
                            <label>Вложения</label>
                            {taskDetails.attachments && taskDetails.attachments.length > 0 ? (
                                <div className="attachments-list">
                                    {taskDetails.attachments.map((attachment) => (
                                        <div key={attachment.id} className="attachment-item">
                                            <span className="attachment-name">{attachment.name}</span>
                                            <button
                                                type="button"
                                                className="attachment-download-btn"
                                                onClick={() => handleDownloadAttachment(attachment.id, attachment.name)}
                                            >
                                                Скачать
                                            </button>
                                        </div>
                                    ))}
                                </div>
                            ) : (
                                <p className="detail-text">Файлы не прикреплены</p>
                            )}
                        </div>

                        <div className="detail-section">
                            <label>Дата создания</label>
                            <p className="detail-text">
                                {new Date(taskDetails.createdDate).toLocaleDateString('ru-RU', {
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
                                <FaTrash/>
                                Удалить
                            </button>
                        </div>
                    </>
                </div>
            </div>

            <TaskCreateForm
                isOpen={isEditFormOpen}
                onClose={handleCloseEditForm}
                projectId={taskDetails.projectId}
                mode="edit"
                task={taskForEdit}
                onSaved={handleClose}
                onAttachmentsChanged={() => selectedTaskId ? loadTaskDetails(selectedTaskId, taskDetails) : undefined}
            />
        </div>
    );
}

export default TaskDetailSidebar;