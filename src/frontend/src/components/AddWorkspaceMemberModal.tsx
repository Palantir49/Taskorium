import {useEffect, useRef, useState} from 'react';
import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle,} from './ui/dialog';
import {Button} from './ui/button';
import {UserResponse} from '../types/'; // сгенерированные типы
import {addUserToWorkspace} from '../api/workSpaceService';
import {fetchAllUsers} from '../api/userService';

// ─── Роли workspace ───────────────────────────────────────────────────────────

const WORKSPACE_ROLES: { value: number; label: string; description: string }[] = [
    {value: 1, label: 'Admin', description: 'Полный CRUD, управление участниками'},
    {value: 2, label: 'Member', description: 'Изменение объектов, ограниченное редактирование'},
    {value: 3, label: 'Viewer', description: 'Только чтение'},
];

// ─── Вспомогательные функции ──────────────────────────────────────────────────

function getInitials(user: UserResponse): string {
    if (user.userName) {
        const parts = user.userName.trim().split(/\s+/);
        if (parts.length >= 2) return (parts[0][0] + parts[1][0]).toUpperCase();
        return user.userName.slice(0, 2).toUpperCase();
    }
    if (user.email) return user.email.slice(0, 2).toUpperCase();
    return '??';
}

function getDisplayName(user: UserResponse): string {
    return user.userName || user.email || user.id;
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

function UserAvatar({user, size = 32}: { user: UserResponse; size?: number }) {
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

// ─── Пропсы ───────────────────────────────────────────────────────────────────

interface AddWorkspaceMemberModalProps {
    isOpen: boolean;
    onOpenChange: (open: boolean) => void;
    workspaceId: string;
    /** id пользователей, уже состоящих в workspace */
    existingMemberIds?: string[];
    onSuccess?: () => void;
}

// ─── Компонент ────────────────────────────────────────────────────────────────

export default function AddWorkspaceMemberModal({
                                                    isOpen,
                                                    onOpenChange,
                                                    workspaceId,
                                                    existingMemberIds = [],
                                                    onSuccess,
                                                }: AddWorkspaceMemberModalProps) {
    const [allUsers, setAllUsers] = useState<UserResponse[]>([]);
    const [search, setSearch] = useState('');
    const [selectedUserId, setSelectedUserId] = useState<string | null>(null);
    const [selectedRole, setSelectedRole] = useState<number>(2);
    const [isLoading, setIsLoading] = useState(false);
    const [isFetching, setIsFetching] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const searchRef = useRef<HTMLInputElement>(null);

    // Загрузка всех пользователей системы
    useEffect(() => {
        if (!isOpen) return;

        const load = async () => {
            setIsFetching(true);
            setError(null);
            try {
                const users = await fetchAllUsers();
                setAllUsers(users);
                setAllUsers(users);
            } catch {
                setError('Не удалось загрузить список пользователей');
            } finally {
                setIsFetching(false);
            }
        };

        load();
    }, [isOpen]);

    // Сброс при закрытии
    useEffect(() => {
        if (!isOpen) {
            setSearch('');
            setSelectedUserId(null);
            setSelectedRole(2);
            setError(null);
        } else {
            setTimeout(() => searchRef.current?.focus(), 50);
        }
    }, [isOpen]);

    // Фильтруем: исключаем уже состоящих + применяем поиск
    const availableUsers = allUsers.filter(
        u => !existingMemberIds.includes(u.id)
    );

    const filteredUsers = availableUsers.filter(u => {
        const q = search.toLowerCase().trim();
        if (!q) return true;
        return (
            u.userName?.toLowerCase().includes(q) ||
            u.email?.toLowerCase().includes(q)
        );
    });

    const selectedUser = allUsers.find(u => u.id === selectedUserId) ?? null;

    const handleAdd = async () => {
        if (!selectedUserId) return;
        setIsLoading(true);
        setError(null);
        try {
            await addUserToWorkspace(workspaceId, {userId: selectedUserId, role: selectedRole});
            onSuccess?.();
            onOpenChange(false);
        } catch {
            setError('Не удалось добавить пользователя в рабочую область');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <Dialog open={isOpen} onOpenChange={onOpenChange}>
            <DialogContent className="sm:max-w-md bg-white text-black border-gray-200">
                <DialogHeader>
                    <DialogTitle>Добавить участника</DialogTitle>
                    <DialogDescription>
                        Выберите пользователя и назначьте роль в рабочей области
                    </DialogDescription>
                </DialogHeader>

                <div className="space-y-4 py-2">

                    {/* Поиск */}
                    <div className="relative">
                        <svg
                            className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 pointer-events-none"
                            width="14" height="14" viewBox="0 0 20 20" fill="currentColor"
                        >
                            <path fillRule="evenodd"
                                  d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                                  clipRule="evenodd"/>
                        </svg>
                        <input
                            ref={searchRef}
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
                        ) : filteredUsers.length === 0 ? (
                            <div className="text-center text-gray-400 text-sm py-6">
                                {availableUsers.length === 0
                                    ? 'Все пользователи уже состоят в рабочей области'
                                    : 'Пользователи не найдены'}
                            </div>
                        ) : (
                            filteredUsers.map(user => {
                                const isSelected = selectedUserId === user.id;
                                return (
                                    <div
                                        key={user.id}
                                        onClick={() => setSelectedUserId(user.id)}
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
                                        <UserAvatar user={user} size={34}/>
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
                                                {user.userName || '—'}
                                            </div>
                                            {user.email && (
                                                <div
                                                    style={{
                                                        fontSize: 12,
                                                        color: '#9ca3af',
                                                        overflow: 'hidden',
                                                        textOverflow: 'ellipsis',
                                                        whiteSpace: 'nowrap',
                                                    }}
                                                >
                                                    {user.email}
                                                </div>
                                            )}
                                        </div>

                                        {/* Радио-индикатор */}
                                        <div
                                            style={{
                                                width: 18,
                                                height: 18,
                                                borderRadius: '50%',
                                                border: isSelected ? '2px solid #6366f1' : '2px solid #d1d5db',
                                                background: isSelected ? '#6366f1' : 'transparent',
                                                display: 'flex',
                                                alignItems: 'center',
                                                justifyContent: 'center',
                                                flexShrink: 0,
                                                transition: 'all 0.15s',
                                            }}
                                        >
                                            {isSelected && (
                                                <div style={{
                                                    width: 7,
                                                    height: 7,
                                                    borderRadius: '50%',
                                                    background: '#fff'
                                                }}/>
                                            )}
                                        </div>
                                    </div>
                                );
                            })
                        )}
                    </div>

                    {/* Блок роли — появляется только после выбора пользователя */}
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
                            <div style={{display: 'flex', alignItems: 'center', gap: 8, marginBottom: 10}}>
                                <UserAvatar user={selectedUser} size={22}/>
                                <span style={{fontSize: 13, fontWeight: 500, color: '#374151'}}>
        {getDisplayName(selectedUser)}
        </span>
                            </div>

                            {/* Выбор роли */}
                            <div style={{fontSize: 12, color: '#6b7280', marginBottom: 6, fontWeight: 500}}>
                                Роль в рабочей области
                            </div>
                            <div style={{display: 'flex', flexDirection: 'column', gap: 6}}>
                                {WORKSPACE_ROLES.map(role => (
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
                                            name="workspace-role"
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
