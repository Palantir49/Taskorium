import React, { useState, useEffect } from 'react';
import {
  FaTimes,
  FaPaperclip
} from 'react-icons/fa';
import { useTasks } from '../context/TaskContext';
import { createTaskInProject } from '../api/taskService';
import { fetchProjectById } from '../api/projectService';
import { fetchIssuePriorities, fetchIssueTypes } from '../api/collectionService';
import { TaskCreateFormProps } from '../types';
import { IssuePriorityResponse, IssueTypeResponse } from '../types/issue';
import './TaskDetailSidebar.css';

function TaskCreateForm({
  isOpen,
  onClose,
  projectId,
  initialStatus = 'backlog',
  mode = 'create',
  task = null,
  onSaved
}: TaskCreateFormProps) {
  const { loadTasks, updateTask } = useTasks();
  const [issueTypes, setIssueTypes] = useState<IssueTypeResponse[]>([]);
  const [issuePriorities, setIssuePriorities] = useState<IssuePriorityResponse[]>([]);
  const [workspaceId, setWorkspaceId] = useState<string>('');
  const [attachments, setAttachments] = useState<File[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    numberIssueType: 0,
    numberIssuePriority: 0,
    dueDate: ''
  });

  useEffect(() => {
    if (!isOpen) return;

    const loadFormData = async () => {
      try {
        const [types, priorities, project] = await Promise.all([
          fetchIssueTypes(),
          fetchIssuePriorities(),
          fetchProjectById(projectId),
        ]);

        setIssueTypes(types);
        setIssuePriorities(priorities);
        setWorkspaceId(project.workspaceId);

        setFormData(
          mode === 'edit' && task
            ? {
                name: task.name || '',
                description: task.description || '',
                numberIssueType: task.issueType?.number ?? (types[0]?.number ?? 0),
                numberIssuePriority: task.issuePriority?.number ?? (priorities[0]?.number ?? 0),
                dueDate: task.dueDate ? task.dueDate.split('T')[0] : ''
              }
            : {
                name: '',
                description: '',
                numberIssueType: types[0]?.number ?? 0,
                numberIssuePriority: priorities[0]?.number ?? 0,
                dueDate: ''
              }
        );
        setAttachments([]);
      } catch (error) {
        console.error('Ошибка загрузки данных для формы создания задачи:', error);
      }
    };

    loadFormData();
  }, [isOpen, initialStatus, projectId, mode, task]);

  if (!isOpen) return null;

  const handleClose = () => {
    onClose();
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'numberIssueType' || name === 'numberIssuePriority' ? Number(value) : value
    }));
  };

  const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files ? Array.from(e.target.files) : [];
    setAttachments(files);
  };

  const handleSave = async () => {
    if (!formData.name.trim()) {
      alert('Название задачи обязательно для заполнения');
      return;
    }

    if (!workspaceId) {
      alert('Не удалось определить рабочую область проекта');
      return;
    }

    setIsLoading(true);
    try {
      if (mode === 'edit' && task) {
        await updateTask(task.id, {
          name: formData.name.trim(),
          description: formData.description.trim() || undefined,
          issueStatusId: task.taskStatusId,
          numberIssueType: formData.numberIssueType,
          numberIssuePriority: formData.numberIssuePriority,
          dueDate: formData.dueDate || null
        });
      } else {
        const taskFormData = new FormData();
        taskFormData.append('name', formData.name.trim());
        taskFormData.append('projectId', projectId);
        taskFormData.append('numberIssueType', formData.numberIssueType.toString());
        taskFormData.append('numberIssuePriority', formData.numberIssuePriority.toString());

        if (formData.description.trim()) {
          taskFormData.append('description', formData.description.trim());
        }
        if (formData.dueDate) {
          taskFormData.append('dueDate', formData.dueDate);
        }

        attachments.forEach((file) => {
          taskFormData.append('attachments', file);
        });

        await createTaskInProject(workspaceId, projectId, taskFormData);
      }

      await loadTasks();
      onSaved?.();
      onClose();
    } catch (error) {
      console.error('Ошибка при сохранении задачи:', error);
      alert(mode === 'edit' ? 'Не удалось обновить задачу' : 'Не удалось создать задачу');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="task-detail-sidebar-overlay" onClick={handleClose}>
      <div className="task-detail-sidebar" onClick={(e) => e.stopPropagation()}>
        <div className="sidebar-header">
          <h2>{mode === 'edit' ? 'Редактировать задачу' : 'Создать задачу'}</h2>
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
              {issueTypes.map((type) => (
                <option key={type.number} value={type.number}>
                  {type.displayName}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Приоритет задачи</label>
            <select
              name="numberIssuePriority"
              value={formData.numberIssuePriority}
              onChange={handleChange}
              className="form-select"
            >
              {issuePriorities.map((priority) => (
                <option key={priority.number} value={priority.number}>
                  {priority.displayName}
                </option>
              ))}
            </select>
          </div>

          {mode === 'create' && (
            <div className="form-group">
              <label>Документы</label>
              <div className="attachments-row">
                <span className="attachments-text">
                  {attachments.length > 0
                    ? attachments.map((f) => f.name).join(', ')
                    : 'Файлы не выбраны'}
                </span>

                <label className="attachment-clip-btn" title="Прикрепить файл">
                  <FaPaperclip />
                  <input
                    type="file"
                    multiple
                    onChange={handleFileSelect}
                    style={{ display: 'none' }}
                  />
                </label>
               </div>
            </div>
          )}

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
              {isLoading ? (mode === 'edit' ? 'Сохранение...' : 'Создание...') : (mode === 'edit' ? 'Сохранить' : 'Создать')}
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