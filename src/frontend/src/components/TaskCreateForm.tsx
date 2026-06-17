import React, {useEffect, useRef, useState} from 'react';
import {FaChevronDown, FaPaperclip, FaSearch, FaTimes as FaTimesCircle, FaTimes, FaUserPlus,} from 'react-icons/fa';
import {useTasks} from '../context/TaskContext';
import {createTaskInProject} from '../api/taskService';
import {deleteAttachment} from '../api/attachmentService';
import {fetchProjectById, fetchProjectMembers} from '../api/projectService';
import {fetchIssuePriorities, fetchIssueTypes} from '../api/collectionService';
import {
    AttachmentResponse,
    IssueAssigneesDto,
    IssuePriorityResponse,
    IssueTypeResponse,
    ProjectUserDto,
    TaskCreateFormProps,
} from '../types';
import './TaskDetailSidebar.css';

type UpdateIssueAssigneePayload = {
    userId: string;
    projectRolesDto: number;
    userName: string;
};

// ─── Вспомогательные функции ──────────────────────────────────────────────────

function getInitials(user: ProjectUserDto): string {
    if (user.userName) {
        const parts = user.userName.trim().split(/\s+/);
        if (parts.length >= 2) return (parts[0][0] + parts[1][0]).toUpperCase();
        return user.userName.slice(0, 2).toUpperCase();
    }
    if (user.email) return user.email.slice(0, 2).toUpperCase();
    return '??';
}

function getDisplayName(user: ProjectUserDto): string {
    return user.userName || user.email || user.id;
}

const AVATAR_COLORS = [
    '#6366f1', '#8b5cf6', '#ec4899', '#14b8a6',
    '#f59e0b', '#3b82f6', '#10b981', '#f97316',
];

function getAvatarColor(id: string): string {
    let hash = 0;
    for (let i = 0; i < id.length; i++) {
        hash = id.charCodeAt(i) + ((hash << 5) - hash);
    }
    return AVATAR_COLORS[Math.abs(hash) % AVATAR_COLORS.length];
}

// ─── Компонент аватара ────────────────────────────────────────────────────────

function UserAvatar({user, size = 28}: { user: ProjectUserDto; size?: number }) {
    return (
        <div
            style={{
                width: size,
                height: size,
                borderRadius: '50%',
                background: getAvatarColor(user.id),
                color: '#fff',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: size * 0.36,
                fontWeight: 600,
                flexShrink: 0,
                userSelect: 'none',
                letterSpacing: '-0.02em',
            }}
            title={getDisplayName(user)}
        >
            {getInitials(user)}
        </div>
    );
}

// ─── Основной компонент ───────────────────────────────────────────────────────

function TaskCreateForm({
                            isOpen,
                            onClose,
                            projectId,
                            initialStatus = 'backlog',
                            mode = 'create',
                            task = null,
                            onSaved,
                        }: TaskCreateFormProps) {
    const {loadTasks, updateTask} = useTasks();

    const [issueTypes, setIssueTypes] = useState<IssueTypeResponse[]>([]);
    const [issuePriorities, setIssuePriorities] = useState<IssuePriorityResponse[]>([]);
    // Полные данные участников проекта — нужны для получения role при тогле
    const [projectMembers, setProjectMembers] = useState<ProjectUserDto[]>([]);
    const [workspaceId, setWorkspaceId] = useState<string>('');
    const [attachments, setAttachments] = useState<File[]>([]);
    const [existingAttachments, setExistingAttachments] = useState<AttachmentResponse[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [assigneeDropdownOpen, setAssigneeDropdownOpen] = useState(false);
    const [memberSearch, setMemberSearch] = useState('');

    const dropdownRef = useRef<HTMLDivElement>(null);
    const searchInputRef = useRef<HTMLInputElement>(null);

    const [formData, setFormData] = useState({
        name: '',
        description: '',
        numberIssueType: 0,
        numberIssuePriority: 0,
        dueDate: '',
        // Массив объектов { userId, role } — именно то что ждёт бэкенд
        assignees: [] as IssueAssigneesDto[],
    });

    // ── Загрузка данных формы ─────────────────────────────────────────────────

    useEffect(() => {
        if (!isOpen) return;

        const loadFormData = async () => {
            try {
                const [types, priorities, project, membersResponse] = await Promise.all([
                    fetchIssueTypes(),
                    fetchIssuePriorities(),
                    fetchProjectById(projectId),
                    fetchProjectMembers(projectId),
                ]);

                setIssueTypes(types);
                setIssuePriorities(priorities);
                setWorkspaceId(project.workspaceId);
                setProjectMembers(membersResponse.members);
                setExistingAttachments(mode === 'edit' && task?.attachments ? [...task.attachments] : []);
                console.log('TaskCreateForm edit task:', task);
                console.log('TaskCreateForm existing attachments:', mode === 'edit' ? task?.attachments : []);

                setFormData(
                    mode === 'edit' && task
                        ? {
                            name: task.name || '',
                            description: task.description || '',
                            numberIssueType: task.issueType?.number ?? (types[0]?.number ?? 0),
                            numberIssuePriority: task.issuePriority?.number ?? (priorities[0]?.number ?? 0),
                            dueDate: task.dueDate ? task.dueDate.split('T')[0] : '',
                            // task.assignees — ProjectUserDto[], собираем IssueAssigneesDto[]
                            // role берём из самого assignee (его роль в проекте)
                            assignees: task.assignees?.map((a: IssueAssigneesDto) => ({
                                userId: a.userId,
                                role: a.role as number,
                                userName: a.userName
                            })) ?? [],
                        }
                        : {
                            name: '',
                            description: '',
                            numberIssueType: types[0]?.number ?? 0,
                            numberIssuePriority: priorities[0]?.number ?? 0,
                            dueDate: '',
                            assignees: [],
                        }
                );

                setAttachments([]);
            } catch (error) {
                console.error('Ошибка загрузки данных для формы:', error);
            }
        };

        loadFormData();
    }, [isOpen, projectId, mode, task]);

    // ── Закрытие дропдауна при клике вне ─────────────────────────────────────

    useEffect(() => {
        if (!assigneeDropdownOpen) return;
        const handler = (e: MouseEvent) => {
            if (dropdownRef.current && !dropdownRef.current.contains(e.target as Node)) {
                setAssigneeDropdownOpen(false);
                setMemberSearch('');
            }
        };
        document.addEventListener('mousedown', handler);
        return () => document.removeEventListener('mousedown', handler);
    }, [assigneeDropdownOpen]);

    useEffect(() => {
        if (assigneeDropdownOpen) {
            setTimeout(() => searchInputRef.current?.focus(), 50);
        }
    }, [assigneeDropdownOpen]);

    if (!isOpen) return null;

    // ── Производные данные ────────────────────────────────────────────────────

    // userId'ы выбранных — для быстрой проверки isSelected
    const selectedUserIds = new Set(formData.assignees.map(a => a.userId));

    // Полные данные выбранных участников — для рендера тегов
    const selectedAssignees = projectMembers.filter(m => selectedUserIds.has(m.id));

    const filteredMembers = projectMembers.filter(m => {
        const q = memberSearch.toLowerCase().trim();
        if (!q) return true;
        return (
            m.userName?.toLowerCase().includes(q) ||
            m.email?.toLowerCase().includes(q)
        );
    });

    // ── Обработчики ───────────────────────────────────────────────────────────

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
    ) => {
        const {name, value} = e.target;
        setFormData(prev => ({
            ...prev,
            [name]:
                name === 'numberIssueType' || name === 'numberIssuePriority'
                    ? Number(value)
                    : value,
        }));
    };

    // Тоггл: добавляем { userId, role } или снимаем по userId.
    // role берём из projectMembers — роль пользователя в проекте.
    const handleAssigneeToggle = (member: ProjectUserDto) => {
        setFormData(prev => {
            const isSelected = prev.assignees.some(a => a.userId === member.id);
            if (isSelected) {
                return {
                    ...prev,
                    assignees: prev.assignees.filter(a => a.userId !== member.id),
                };
            }
            return {
                ...prev,
                assignees: [
                    ...prev.assignees,
                    {userId: member.id, role: member.role as number, userName: member.userName},
                ],
            };
        });
    };

    const handleAssigneeRemove = (userId: string) => {
        setFormData(prev => ({
            ...prev,
            assignees: prev.assignees.filter(a => a.userId !== userId),
        }));
    };

    const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
        const files = e.target.files ? Array.from(e.target.files) : [];
        if (!files.length) return;
        setAttachments(prev => {
            const existingKeys = new Set(prev.map(f => `${f.name}_${f.size}_${f.lastModified}`));
            return [
                ...prev,
                ...files.filter(f => !existingKeys.has(`${f.name}_${f.size}_${f.lastModified}`)),
            ];
        });
        e.target.value = '';
    };

    const handleRemoveAttachment = (indexToRemove: number) => {
        setAttachments(prev => prev.filter((_, i) => i !== indexToRemove));
    };

    const handleDeleteExistingAttachment = async (attachmentId: string) => {
        if (!window.confirm('Удалить вложение?')) return;

        try {
            await deleteAttachment(attachmentId);
            setExistingAttachments((prev) => prev.filter((attachment) => attachment.id !== attachmentId));
        } catch (error) {
            console.error('Ошибка удаления вложения:', error);
            alert('Не удалось удалить файл');
        }
    };

    const formatFileSize = (bytes: number): string => {
        if (bytes < 1024) return `${bytes} Б`;
        if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} КБ`;
        return `${(bytes / (1024 * 1024)).toFixed(1)} МБ`;
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
                const updateAssigneesPayload: UpdateIssueAssigneePayload[] = formData.assignees.map((assignee) => ({
                    userId: assignee.userId,
                    projectRolesDto: assignee.role,
                    userName: assignee.userName,
                }));

                await updateTask(task.id, {
                    name: formData.name.trim(),
                    description: formData.description.trim() || undefined,
                    issueStatusId: task.taskStatusId,
                    numberIssueType: formData.numberIssueType,
                    numberIssuePriority: formData.numberIssuePriority,
                    dueDate: formData.dueDate || null,
                    assignees: updateAssigneesPayload as never,
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

                formData.assignees.forEach((assignee, i) => {
                    taskFormData.append(`Assignees[${i}].UserId`, assignee.userId);
                    taskFormData.append(`Assignees[${i}].ProjectRolesDto`, assignee.role.toString());
                    taskFormData.append(`Assignees[${i}].UserName`, assignee.userName);
                });

                attachments.forEach(file => {
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

    // ── Рендер ────────────────────────────────────────────────────────────────

    return (
        <div className="task-detail-sidebar-overlay" onClick={onClose}>
            <div className="task-detail-sidebar" onClick={e => e.stopPropagation()}>

                <div className="sidebar-header">
                    <h2>{mode === 'edit' ? 'Редактировать задачу' : 'Создать задачу'}</h2>
                    <button className="close-btn" onClick={onClose}>
                        <FaTimes/>
                    </button>
                </div>

                <div className="sidebar-content">

                    {/* Название */}
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

                    {/* Описание */}
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

                    {/* Тип задачи */}
                    <div className="form-group">
                        <label>Тип задачи</label>
                        <select
                            name="numberIssueType"
                            value={formData.numberIssueType}
                            onChange={handleChange}
                            className="form-select"
                        >
                            {issueTypes.map(type => (
                                <option key={type.number} value={type.number}>
                                    {type.displayName}
                                </option>
                            ))}
                        </select>
                    </div>

                    {/* Приоритет */}
                    <div className="form-group">
                        <label>Приоритет задачи</label>
                        <select
                            name="numberIssuePriority"
                            value={formData.numberIssuePriority}
                            onChange={handleChange}
                            className="form-select"
                        >
                            {issuePriorities.map(priority => (
                                <option key={priority.number} value={priority.number}>
                                    {priority.displayName}
                                </option>
                            ))}
                        </select>
                    </div>

                    {/* Исполнители */}
                    <div className="form-group">
                        <label>Исполнители</label>
                        <div ref={dropdownRef} style={{position: 'relative'}}>

                            {/* Кнопка-триггер */}
                            <button
                                type="button"
                                className="form-input"
                                style={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    justifyContent: 'space-between',
                                    gap: 8,
                                    cursor: 'pointer',
                                    width: '100%',
                                    textAlign: 'left',
                                    minHeight: 38,
                                    flexWrap: 'nowrap',
                                }}
                                onClick={() => setAssigneeDropdownOpen(v => !v)}
                            >
                                <span
                                    style={{
                                        display: 'flex',
                                        alignItems: 'center',
                                        gap: 6,
                                        flexWrap: 'wrap',
                                        flex: 1,
                                        minWidth: 0,
                                    }}
                                >
                                    {selectedAssignees.length > 0 ? (
                                        selectedAssignees.map(user => (
                                            <span
                                                key={user.id}
                                                style={{
                                                    display: 'inline-flex',
                                                    alignItems: 'center',
                                                    gap: 4,
                                                    background: 'var(--bg-tag, #eef2ff)',
                                                    color: 'var(--text-tag, #4338ca)',
                                                    borderRadius: 20,
                                                    padding: '2px 6px 2px 3px',
                                                    fontSize: 12,
                                                    fontWeight: 500,
                                                    whiteSpace: 'nowrap',
                                                    maxWidth: 160,
                                                }}
                                            >
                                                <UserAvatar user={user} size={17}/>
                                                <span style={{
                                                    overflow: 'hidden',
                                                    textOverflow: 'ellipsis',
                                                    maxWidth: 100
                                                }}>
                                                    {getDisplayName(user)}
                                                </span>
                                                <span
                                                    onMouseDown={e => {
                                                        e.stopPropagation();
                                                        handleAssigneeRemove(user.id);
                                                    }}
                                                    style={{
                                                        cursor: 'pointer',
                                                        opacity: 0.5,
                                                        display: 'flex',
                                                        alignItems: 'center',
                                                        flexShrink: 0
                                                    }}
                                                    title="Убрать"
                                                >
                                                    <FaTimesCircle size={10}/>
                                                </span>
                                            </span>
                                        ))
                                    ) : (
                                        <span style={{
                                            display: 'flex',
                                            alignItems: 'center',
                                            gap: 6,
                                            color: 'var(--text-muted, #9ca3af)'
                                        }}>
                                            <FaUserPlus size={13} style={{opacity: 0.5}}/>
                                            <span style={{fontSize: 14}}>Не назначен</span>
                                        </span>
                                    )}
                                </span>
                                <FaChevronDown
                                    size={11}
                                    style={{
                                        opacity: 0.4,
                                        flexShrink: 0,
                                        transform: assigneeDropdownOpen ? 'rotate(180deg)' : 'rotate(0deg)',
                                        transition: 'transform 0.15s ease',
                                    }}
                                />
                            </button>

                            {/* Дропдаун */}
                            {assigneeDropdownOpen && (
                                <div
                                    style={{
                                        position: 'absolute',
                                        zIndex: 200,
                                        top: 'calc(100% + 4px)',
                                        left: 0,
                                        right: 0,
                                        background: 'var(--bg-surface, #ffffff)',
                                        border: '1px solid var(--border-color, #e5e7eb)',
                                        borderRadius: 8,
                                        boxShadow: '0 4px 20px rgba(0,0,0,0.12)',
                                        overflow: 'hidden',
                                    }}
                                >
                                    {/* Поиск */}
                                    <div style={{
                                        padding: '8px 8px 6px',
                                        borderBottom: '1px solid var(--border-color, #f3f4f6)'
                                    }}>
                                        <div style={{position: 'relative'}}>
                                            <FaSearch
                                                size={11}
                                                style={{
                                                    position: 'absolute', left: 10, top: '50%',
                                                    transform: 'translateY(-50%)',
                                                    color: 'var(--text-muted, #9ca3af)',
                                                    pointerEvents: 'none',
                                                }}
                                            />
                                            <input
                                                ref={searchInputRef}
                                                type="text"
                                                placeholder="Поиск участника..."
                                                value={memberSearch}
                                                onChange={e => setMemberSearch(e.target.value)}
                                                className="form-input"
                                                style={{
                                                    padding: '6px 10px 6px 28px',
                                                    fontSize: 13,
                                                    width: '100%',
                                                    boxSizing: 'border-box'
                                                }}
                                            />
                                        </div>
                                    </div>

                                    {/* Список участников */}
                                    <ul style={{
                                        listStyle: 'none',
                                        margin: 0,
                                        padding: '4px 0',
                                        maxHeight: 220,
                                        overflowY: 'auto'
                                    }}>
                                        {filteredMembers.length === 0 ? (
                                            <li style={{
                                                padding: '10px 14px',
                                                color: 'var(--text-muted, #9ca3af)',
                                                fontSize: 13,
                                                textAlign: 'center'
                                            }}>
                                                Участники не найдены
                                            </li>
                                        ) : (
                                            filteredMembers.map(member => {
                                                const isSelected = selectedUserIds.has(member.id);
                                                return (
                                                    <li
                                                        key={member.id}
                                                        // Передаём весь member — чтобы взять role при добавлении
                                                        onMouseDown={() => handleAssigneeToggle(member)}
                                                        style={{
                                                            display: 'flex',
                                                            alignItems: 'center',
                                                            gap: 10,
                                                            padding: '7px 12px',
                                                            cursor: 'pointer',
                                                            fontSize: 14,
                                                            background: isSelected ? 'var(--bg-selected, #eef2ff)' : 'transparent',
                                                            transition: 'background 0.1s',
                                                        }}
                                                        onMouseEnter={e => (e.currentTarget.style.background = isSelected ? 'var(--bg-selected, #eef2ff)' : 'var(--bg-hover, #f9fafb)')}
                                                        onMouseLeave={e => (e.currentTarget.style.background = isSelected ? 'var(--bg-selected, #eef2ff)' : 'transparent')}
                                                    >
                                                        <UserAvatar user={member} size={28}/>
                                                        <div style={{
                                                            flex: 1,
                                                            display: 'flex',
                                                            flexDirection: 'column',
                                                            lineHeight: 1.35,
                                                            minWidth: 0
                                                        }}>
                                                            <span style={{
                                                                fontWeight: 500,
                                                                overflow: 'hidden',
                                                                textOverflow: 'ellipsis',
                                                                whiteSpace: 'nowrap'
                                                            }}>
                                                                {member.userName || '—'}
                                                            </span>
                                                            {member.email && (
                                                                <span style={{
                                                                    fontSize: 12,
                                                                    color: 'var(--text-muted, #9ca3af)',
                                                                    overflow: 'hidden',
                                                                    textOverflow: 'ellipsis',
                                                                    whiteSpace: 'nowrap'
                                                                }}>
                                                                    {member.email}
                                                                </span>
                                                            )}
                                                        </div>
                                                        {/* Чекбокс */}
                                                        <div
                                                            style={{
                                                                width: 18,
                                                                height: 18,
                                                                borderRadius: 4,
                                                                border: isSelected ? '2px solid var(--color-primary, #6366f1)' : '2px solid var(--border-color, #d1d5db)',
                                                                background: isSelected ? 'var(--color-primary, #6366f1)' : 'transparent',
                                                                display: 'flex',
                                                                alignItems: 'center',
                                                                justifyContent: 'center',
                                                                flexShrink: 0,
                                                                transition: 'all 0.15s',
                                                            }}
                                                        >
                                                            {isSelected && (
                                                                <svg width="10" height="8" viewBox="0 0 10 8"
                                                                     fill="none">
                                                                    <path d="M1 4L3.5 6.5L9 1" stroke="white"
                                                                          strokeWidth="1.8" strokeLinecap="round"
                                                                          strokeLinejoin="round"/>
                                                                </svg>
                                                            )}
                                                        </div>
                                                    </li>
                                                );
                                            })
                                        )}
                                    </ul>

                                    {/* Счётчик + снять всех */}
                                    {formData.assignees.length > 0 && (
                                        <div
                                            style={{
                                                padding: '6px 12px',
                                                borderTop: '1px solid var(--border-color, #f3f4f6)',
                                                fontSize: 12,
                                                color: 'var(--text-muted, #9ca3af)',
                                                display: 'flex',
                                                justifyContent: 'space-between',
                                                alignItems: 'center',
                                            }}
                                        >
                                            <span>Выбрано: {formData.assignees.length}</span>
                                            <button
                                                type="button"
                                                onMouseDown={e => {
                                                    e.stopPropagation();
                                                    setFormData(prev => ({...prev, assignees: []}));
                                                }}
                                                style={{
                                                    background: 'none',
                                                    border: 'none',
                                                    cursor: 'pointer',
                                                    color: 'var(--color-danger, #ef4444)',
                                                    fontSize: 12,
                                                    padding: '2px 4px'
                                                }}
                                            >
                                                Снять всех
                                            </button>
                                        </div>
                                    )}
                                </div>
                            )}
                        </div>
                    </div>

                    <div className="form-group">
                        <label>Документы</label>

                        {mode === 'edit' && existingAttachments.length > 0 && (
                            <div className="attachments-list" style={{marginBottom: 12}}>
                                {existingAttachments.map((attachment) => (
                                    <div key={attachment.id} className="attachment-item">
                                        <span className="attachment-name">{attachment.name}</span>
                                        <button
                                            type="button"
                                            className="attachment-delete-btn"
                                            onClick={() => handleDeleteExistingAttachment(attachment.id)}
                                        >
                                            Удалить
                                        </button>
                                    </div>
                                ))}
                            </div>
                        )}

                        {mode === 'create' && (
                            <div className="attachments-row" style={{alignItems: 'flex-start'}}>
                                <div className="attachments-text" style={{flex: 1}}>
                                    {attachments.length > 0 ? (
                                        attachments.map((file, index) => (
                                            <div
                                                key={`${file.name}_${file.size}_${file.lastModified}`}
                                                style={{
                                                    display: 'flex',
                                                    justifyContent: 'space-between',
                                                    gap: 8,
                                                    marginBottom: 4
                                                }}
                                            >
                                                <span>{file.name} ({formatFileSize(file.size)})</span>
                                                <button type="button" onClick={() => handleRemoveAttachment(index)}>🗑️
                                                </button>
                                            </div>
                                        ))
                                    ) : 'Файлы не выбраны'}
                                </div>
                                <label className="attachment-clip-btn" title="Прикрепить файл">
                                    <FaPaperclip/>
                                    <input type="file" multiple onChange={handleFileSelect} style={{display: 'none'}}/>
                                </label>
                            </div>
                        )}

                        {mode === 'edit' && existingAttachments.length === 0 && (
                            <p className="attachments-text">Файлы не прикреплены</p>
                        )}
                    </div>

                    {/* Дедлайн */}
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

                    {/* Действия */}
                    <div className="form-actions">
                        <button className="btn-save" onClick={handleSave} disabled={isLoading}>
                            {isLoading
                                ? mode === 'edit' ? 'Сохранение...' : 'Создание...'
                                : mode === 'edit' ? 'Сохранить' : 'Создать'}
                        </button>
                        <button className="btn-cancel" onClick={onClose} disabled={isLoading}>
                            Отмена
                        </button>
                    </div>

                </div>
            </div>
        </div>
    );
}

export default TaskCreateForm;