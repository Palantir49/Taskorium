// ─── MembersPreview.tsx ───────────────────────────────────────────────────────
// Строка с аватарами участников на карточке.
// Клик открывает модалку со списком.

import {useState} from 'react';
import {Dialog, DialogContent, DialogHeader, DialogTitle,} from './ui/dialog';

// ─── Типы ─────────────────────────────────────────────────────────────────────

export interface MemberItem {
    id: string;
    userName?: string | null;
    email?: string | null;
    roleName?: string; // отображаемое название роли
}

interface MembersPreviewProps {
    members: MemberItem[];
    entityName: string; // для заголовка модалки: "Проект X" или "Рабочая область Y"
    isLoading?: boolean;
}

// ─── Вспомогательные функции ──────────────────────────────────────────────────

function getInitials(member: MemberItem): string {
    if (member.userName) {
        const parts = member.userName.trim().split(/\s+/);
        if (parts.length >= 2) return (parts[0][0] + parts[1][0]).toUpperCase();
        return member.userName.slice(0, 2).toUpperCase();
    }
    if (member.email) return member.email.slice(0, 2).toUpperCase();
    return '??';
}

function getDisplayName(member: MemberItem): string {
    return member.userName || member.email || member.id;
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

function Avatar({
                    member,
                    size = 24,
                    style,
                }: {
    member: MemberItem;
    size?: number;
    style?: React.CSSProperties;
}) {
    return (
        <div
            title={getDisplayName(member)}
            style={{
                width: size,
                height: size,
                borderRadius: '50%',
                background: getAvatarColor(member.id),
                color: '#fff',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: size * 0.36,
                fontWeight: 600,
                flexShrink: 0,
                userSelect: 'none',
                letterSpacing: '-0.02em',
                border: '2px solid #fff',
                ...style,
            }}
        >
            {getInitials(member)}
        </div>
    );
}

// ─── Основной компонент ───────────────────────────────────────────────────────

const MAX_VISIBLE = 4;

export default function MembersPreview({
                                           members,
                                           entityName,
                                           isLoading = false,
                                       }: MembersPreviewProps) {
    const [modalOpen, setModalOpen] = useState(false);

    const visible = members.slice(0, MAX_VISIBLE);
    const rest = members.length - MAX_VISIBLE;

    if (isLoading) {
        return (
            <div className="flex items-center gap-2 mt-3 pt-3 border-t border-gray-100">
                <div className="flex">
                    {[0, 1, 2].map(i => (
                        <div
                            key={i}
                            style={{
                                width: 24,
                                height: 24,
                                borderRadius: '50%',
                                background: '#e5e7eb',
                                border: '2px solid #fff',
                                marginLeft: i > 0 ? -6 : 0,
                            }}
                        />
                    ))}
                </div>
                <span className="text-xs text-gray-400">Загрузка...</span>
            </div>
        );
    }

    if (members.length === 0) {
        return (
            <div className="flex items-center gap-1 mt-3 pt-3 border-t border-gray-100">
                <svg width="13" height="13" viewBox="0 0 20 20" fill="currentColor" className="text-gray-300">
                    <path
                        d="M9 6a3 3 0 11-6 0 3 3 0 016 0zM17 6a3 3 0 11-6 0 3 3 0 016 0zM12.93 17c.046-.327.07-.66.07-1a6.97 6.97 0 00-1.5-4.33A5 5 0 0119 16v1h-6.07zM6 11a5 5 0 015 5v1H1v-1a5 5 0 015-5z"/>
                </svg>
                <span className="text-xs text-gray-400">Нет участников</span>
            </div>
        );
    }

    return (
        <>
            <button
                type="button"
                onClick={e => {
                    e.stopPropagation();
                    setModalOpen(true);
                }}
                className="flex items-center gap-2 mt-3 pt-3 border-t border-gray-100 w-full text-left group"
            >
                {/* Стек аватаров */}
                <div className="flex">
                    {visible.map((member, i) => (
                        <Avatar
                            key={member.id}
                            member={member}
                            size={24}
                            style={{marginLeft: i > 0 ? -6 : 0, zIndex: MAX_VISIBLE - i}}
                        />
                    ))}
                    {rest > 0 && (
                        <div
                            style={{
                                width: 24,
                                height: 24,
                                borderRadius: '50%',
                                background: '#f3f4f6',
                                color: '#6b7280',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                fontSize: 9,
                                fontWeight: 600,
                                border: '2px solid #fff',
                                marginLeft: -6,
                                flexShrink: 0,
                            }}
                        >
                            +{rest}
                        </div>
                    )}
                </div>

                {/* Подпись */}
                <span className="text-xs text-gray-400 group-hover:text-indigo-500 transition-colors">
          {members.length} {declineParticipants(members.length)}
        </span>
            </button>

            {/* Модалка со списком */}
            <Dialog open={modalOpen} onOpenChange={setModalOpen}>
                <DialogContent
                    className="sm:max-w-sm bg-white text-black border-gray-200"
                    onClick={e => e.stopPropagation()}
                >
                    <DialogHeader>
                        <DialogTitle className="text-base">
                            Участники — {entityName}
                        </DialogTitle>
                    </DialogHeader>

                    <div
                        style={{
                            maxHeight: 360,
                            overflowY: 'auto',
                            margin: '4px 0',
                        }}
                    >
                        {members.map((member, i) => (
                            <div
                                key={member.id}
                                style={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    gap: 10,
                                    padding: '8px 2px',
                                    borderBottom: i < members.length - 1 ? '1px solid #f3f4f6' : 'none',
                                }}
                            >
                                <Avatar member={member} size={34} style={{border: 'none'}}/>
                                <div style={{flex: 1, minWidth: 0}}>
                                    <div
                                        style={{
                                            fontSize: 14,
                                            fontWeight: 500,
                                            color: '#111827',
                                            overflow: 'hidden',
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'nowrap',
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
                                {member.roleName && (
                                    <span
                                        style={{
                                            fontSize: 11,
                                            fontWeight: 500,
                                            color: '#6366f1',
                                            background: '#eef2ff',
                                            borderRadius: 20,
                                            padding: '2px 8px',
                                            whiteSpace: 'nowrap',
                                            flexShrink: 0,
                                        }}
                                    >
                    {member.roleName}
                  </span>
                                )}
                            </div>
                        ))}
                    </div>
                </DialogContent>
            </Dialog>
        </>
    );
}

// ─── Склонение ────────────────────────────────────────────────────────────────

function declineParticipants(n: number): string {
    const mod10 = n % 10;
    const mod100 = n % 100;
    if (mod100 >= 11 && mod100 <= 14) return 'участников';
    if (mod10 === 1) return 'участник';
    if (mod10 >= 2 && mod10 <= 4) return 'участника';
    return 'участников';
}
