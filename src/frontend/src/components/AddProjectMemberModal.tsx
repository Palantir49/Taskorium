import {useEffect, useState} from 'react';
import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle,} from './ui/dialog';
import {Button} from './ui/button';
import {addUserToProject} from '../api/projectService';
import {fetchWorkspaceMembers} from '../api/workSpaceService.ts';
import {WorkspaceUserDto} from '../types';

// ─── Роли проекта ─────────────────────────────────────────────────────────────

const PROJECT_ROLES = [
    {value: 1, label: 'Admin', description: 'Полный CRUD, назначение участников'},
    {value: 2, label: 'Member', description: 'Изменение статусов, ограниченное редактирование'},
    {value: 3, label: 'Viewer', description: 'Только чтение'},
] as const;

type ProjectRoleValue = typeof PROJECT_ROLES[number]['value'];

// ─── Вспомогательные функции ──────────────────────────────────────────────────

function getInitials(user: WorkspaceUserDto): string {
    if (user.userName) {
        const parts = user.userName.trim().split(/\s+/);
        if (parts.length >= 2) return (parts[0][0] + parts[1][0]).toUpperCase();
        return user.userName.slice(0, 2).toUpperCase();
    }
    if (user.email) return user.email.slice(0, 2).toUpperCase();
    return '??';
}

function getDisplayName(user: WorkspaceUserDto): string {
    return user.userName || user.email || String(user.id);
}

const AVATAR_COLORS = [
    '#6366f1', '#8b5cf6', '#ec4899', '#14b8a6',
    '#f59e0b', '#3b82f6', '#10b981', '#f97316',
];

function getAvatarColor(id: string): string {
    let hash = 0;
    for (let i = 0; i < id.length; i++) hash = id.charCodeAt(i) + ((hash << 5) - hash);
    return AVATAR_COLORS[Math.abs(hash) % AVATAR_COLORS.length];
}

// ─── Аватар ───────────────────────────────────────────────────────────────────

function UserAvatar({user, size = 32}: { user: WorkspaceUserDto; size?: number }) {
    return (
        <div
            style={{
                width: size,
                height: size,
                borderRadius: '50%',
                background: getAvatarColor(String(user.id)),
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

// ─── Пропсы ───────────────────────────────────────────────────────────────────

interface AddProjectMemberModalProps {
    isOpen: boolean;
    onOpenChange: (open: boolean) => void;
    projectId: string;
    workspaceId: string;
    /** id пользователей, уже состоящих в проекте — чтобы их не показывать */
    existingMemberIds?: string[];
    onSuccess?: () => void;
}

// ─── Компонент ────────────────────────────────────────────────────────────────

export default function AddProjectMemberModal({
                                                  isOpen,
                                                  onOpenChange,
                                                  projectId,
                                                  workspaceId,
                                                  existingMemberIds = [],
                                                  onSuccess,
                                              }: AddProjectMemberModalProps) {
    const [workspaceMembers, setWorkspaceMembers] = useState<WorkspaceUserDto[]>([]);
    const [search, setSearch] = useState('');
    const [selectedUserId, setSelectedUserId] = useState<string | null>(null);
    const [selectedRole, setSelectedRole] = useState<ProjectRoleValue>(2);
    const [isLoading, setIsLoading] = useState(false);
    const [isFetching, setIsFetching] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // Загрузка участников рабочей области
    useEffect(() => {
        if (!isOpen || !workspaceId) return;

        const load = async () => {
            setIsFetching(true);
            setError(null);
            try {
                const response = await fetchWorkspaceMembers(workspaceId);
                // Фильтруем тех, кто уже в проекте
                const available = response.members.filter(
                    m => !existingMemberIds.includes(String(m.id))
                );
                setWorkspaceMembers(available);
            } catch {
                setError('Не удалось загрузить участников рабочей области');
            } finally {
                setIsFetching(false);
            }
        };

        load();
    }, [isOpen, workspaceId]);

    // Сброс при закрытии
    useEffect(() => {
        if (!isOpen) {
            setSearch('');
            setSelectedUserId(null);
            setSelectedRole(2);
            setError(null);
        }
    }, [isOpen]);

    const filteredMembers = workspaceMembers.filter(m => {
        const q = search.toLowerCase().trim();
        if (!q) return true;
        return (
            m.userName?.toLowerCase().includes(q) ||
            m.email?.toLowerCase().includes(q)
        );
    });

    const selectedUser = workspaceMembers.find(m => String(m.id) === selectedUserId) ?? null;

    const handleAdd = async () => {
        if (!selectedUserId) return;
        setIsLoading(true);
        setError(null);
        try {
            await addUserToProject(projectId, {
                userId: selectedUserId,
                roleDto: selectedRole,
            });
            onSuccess?.();
            onOpenChange(false);
        } catch {
            setError('Не удалось добавить пользователя в проект');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <Dialog open={isOpen} onOpenChange={onOpenChange}>
            <DialogContent className="sm:max-w-md bg-white text-black border-gray-200">
                <DialogHeader>
                    <DialogTitle>Добавить участника в проект</DialogTitle>
                    <DialogDescription>
                        Выберите пользователя из рабочей области и назначьте ему роль
                    </DialogDescription>
                </DialogHeader>

                <div className="space-y-4 py-2">

                    {/* Поиск */}
                    <div className="relative">
                        <svg
                            className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"
                            width="14" height="14" viewBox="0 0 20 20" fill="currentColor"
                        >
                            <path fillRule="evenodd"
                                  d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                                  clipRule="evenodd"/>
                        </svg>
                        <input
                            type="text"
                            placeholder="Поиск по имени или email..."
                            value={search}
                            onChange={e => setSearch(e.target.value)}
                            className="w-full border border-gray-200 rounded-md pl-9 pr-3 py-2 text-sm outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
                        />
                    </div>

                    {/* Список пользователей */}
                    <div
                        style={{
                            maxHeight: 220,
                            overflowY: 'auto',
                            border: '1px solid #e5e7eb',
                            borderRadius: 8,
                        }}
                    >
                        {isFetching ? (
                            <div className="text-center text-gray-400 text-sm py-6">
                                Загрузка...
                            </div>
                        ) : filteredMembers.length === 0 ? (
                            <div className="text-center text-gray-400 text-sm py-6">
                                {workspaceMembers.length === 0
                                    ? 'Все участники рабочей области уже в проекте'
                                    : 'Пользователи не найдены'}
                            </div>
                        ) : (
                            filteredMembers.map(member => {
                                const isSelected = selectedUserId === String(member.id);
                                return (
                                    <div
                                        key={String(member.id)}
                                        onClick={() => setSelectedUserId(String(member.id))}
                                        style={{
                                            display: 'flex',
                                            alignItems: 'center',
                                            gap: 10,
                                            padding: '8px 12px',
                                            cursor: 'pointer',
                                            background: isSelected ? '#eef2ff' : 'transparent',
                                            borderBottom: '1px solid #f3f4f6',
                                            transition: 'background 0.1s',
                                        }}
                                        onMouseEnter={e => {
                                            if (!isSelected) e.currentTarget.style.background = '#f9fafb';
                                        }}
                                        onMouseLeave={e => {
                                            e.currentTarget.style.background = isSelected ? '#eef2ff' : 'transparent';
                                        }}
                                    >
                                        <UserAvatar user={member} size={34}/>
                                        <div style={{flex: 1, minWidth: 0}}>
                                            <div
                                                style={{
                                                    fontWeight: 500,
                                                    fontSize: 14,
                                                    overflow: 'hidden',
                                                    textOverflow: 'ellipsis',
                                                    whiteSpace: 'nowrap',
                                                    color: '#111827',
                                                }}
                                            >
                                                {member.userName || '—'}
                                            </div>
                                            {member.email && (
                                                <div
                                                    style={{
                                                        fontSize: 12,
                                                        color: '#9ca3af',
                                                        overflow: 'hidden',
                                                        textOverflow: 'ellipsis',
                                                        whiteSpace: 'nowrap',
                                                    }}
                                                >
                                                    {member.email}
                                                </div>
                                            )}
                                        </div>

                                        {/* Радио-индикатор */}
                                        <div
                                            style={{
                                                width: 18,
                                                height: 18,
                                                borderRadius: '50%',
                                                border: isSelected
                                                    ? '2px solid #6366f1'
                                                    : '2px solid #d1d5db',
                                                background: isSelected ? '#6366f1' : 'transparent',
                                                display: 'flex',
                                                alignItems: 'center',
                                                justifyContent: 'center',
                                                flexShrink: 0,
                                                transition: 'all 0.15s',
                                            }}
                                        >
                                            {isSelected && (
                                                <div
                                                    style={{
                                                        width: 7,
                                                        height: 7,
                                                        borderRadius: '50%',
                                                        background: '#fff',
                                                    }}
                                                />
                                            )}
                                        </div>
                                    </div>
                                );
                            })
                        )}
                    </div>

                    {/* Выбранный пользователь + роль */}
                    {selectedUser && (
                        <div
                            style={{
                                background: '#f8faff',
                                border: '1px solid #e0e7ff',
                                borderRadius: 8,
                                padding: '12px 14px',
                            }}
                        >
                            {/* Кто выбран */}
                            <div
                                style={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    gap: 8,
                                    marginBottom: 10,
                                }}
                            >
                                <UserAvatar user={selectedUser} size={24}/>
                                <span style={{fontSize: 13, fontWeight: 500, color: '#374151'}}>
                  {getDisplayName(selectedUser)}
                </span>
                            </div>

                            {/* Выбор роли */}
                            <div style={{fontSize: 12, color: '#6b7280', marginBottom: 6, fontWeight: 500}}>
                                Роль в проекте
                            </div>
                            <div style={{display: 'flex', flexDirection: 'column', gap: 6}}>
                                {PROJECT_ROLES.map(role => (
                                    <label
                                        key={role.value}
                                        style={{
                                            display: 'flex',
                                            alignItems: 'flex-start',
                                            gap: 8,
                                            cursor: 'pointer',
                                            padding: '6px 8px',
                                            borderRadius: 6,
                                            background: selectedRole === role.value ? '#eef2ff' : 'transparent',
                                            border: selectedRole === role.value
                                                ? '1px solid #c7d2fe'
                                                : '1px solid transparent',
                                            transition: 'all 0.1s',
                                        }}
                                    >
                                        <input
                                            type="radio"
                                            name="role"
                                            value={role.value}
                                            checked={selectedRole === role.value}
                                            onChange={() => setSelectedRole(role.value)}
                                            style={{marginTop: 2, accentColor: '#6366f1'}}
                                        />
                                        <div>
                                            <div style={{fontSize: 13, fontWeight: 500, color: '#111827'}}>
                                                {role.label}
                                            </div>
                                            <div style={{fontSize: 11, color: '#9ca3af'}}>
                                                {role.description}
                                            </div>
                                        </div>
                                    </label>
                                ))}
                            </div>
                        </div>
                    )}

                    {/* Ошибка */}
                    {error && (
                        <div
                            style={{
                                fontSize: 13,
                                color: '#ef4444',
                                background: '#fef2f2',
                                border: '1px solid #fecaca',
                                borderRadius: 6,
                                padding: '8px 12px',
                            }}
                        >
                            {error}
                        </div>
                    )}
                </div>

                <DialogFooter>
                    <Button variant="outline" onClick={() => onOpenChange(false)} disabled={isLoading}>
                        Отмена
                    </Button>
                    <Button
                        onClick={handleAdd}
                        disabled={!selectedUserId || isLoading}
                        style={{
                            background: selectedUserId ? '#6366f1' : undefined,
                            color: selectedUserId ? '#fff' : undefined,
                        }}
                    >
                        {isLoading ? 'Добавление...' : 'Добавить'}
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
