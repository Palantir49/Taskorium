import React from 'react';
import axios from 'axios';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from './ui/dialog';
import { fetchIssueStatusesByProjectId } from '../api/projectService';
import { createIssueStatus, deleteIssueStatus } from '../api/issueStatusService';
import { IssueStatusResponse } from '../types/issueStatus';

interface ProjectSettingsModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  projectId: string;
}

export default function ProjectSettingsModal({ open, onOpenChange, projectId }: ProjectSettingsModalProps) {
  const [statuses, setStatuses] = React.useState<IssueStatusResponse[]>([]);
  const [loading, setLoading] = React.useState(false);
  const [newStatusName, setNewStatusName] = React.useState('');
  const [newStatusColor, setNewStatusColor] = React.useState('#3b82f6');
  const [createError, setCreateError] = React.useState<string | null>(null);
  const [deleteError, setDeleteError] = React.useState<string | null>(null);
  const [isCreating, setIsCreating] = React.useState(false);
  const [statusToDeleteId, setStatusToDeleteId] = React.useState<string | null>(null);

  const handleCreateStatus = async () => {
    const trimmedName = newStatusName.trim();
    if (!trimmedName) {
      setCreateError('Введите название статуса');
      return;
    }

    const nextPosition = statuses.length > 0
      ? Math.max(...statuses.map((s) => s.position)) + 1
      : 0;

    setCreateError(null);
    setIsCreating(true);
    try {
      const created = await createIssueStatus({
        name: trimmedName,
        projectId,
        numberType: 1,
        position: nextPosition,
        color: newStatusColor,
      });

      setStatuses((prev) => [...prev, created]);
      setNewStatusName('');
      setNewStatusColor('#3b82f6');
    } catch (error) {
      console.error('Ошибка создания статуса:', error);
      setCreateError('Не удалось создать статус');
    } finally {
      setIsCreating(false);
    }
  };

  const handleDeleteStatus = async (statusId: string) => {
    try {
      setDeleteError(null);
      await deleteIssueStatus(statusId);
      setStatuses((prev) => prev.filter((item) => item.id !== statusId));
      setStatusToDeleteId(null);
    } catch (error) {
      console.error('Ошибка удаления статуса:', error);
      const message =axios.isAxiosError(error) ? 
      error.response?.data?.detail || 'Не удалось удалить статус' : 'Не удалось удалить статус';
      setDeleteError(message);

    }
  };

  React.useEffect(() => {
    if (!open) return;

    setStatusToDeleteId(null);
    setCreateError(null);
    setDeleteError(null);
    const loadStatuses = async () => {
      setLoading(true);
      try {
        const data = await fetchIssueStatusesByProjectId(projectId);
        setStatuses(data);
      } finally {
        setLoading(false);
      }
    };

    loadStatuses();
  }, [open, projectId]);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl bg-white text-black border-gray-300">
        <DialogHeader>
          <DialogTitle className="text-xl font-bold">Настройки проекта</DialogTitle>
        </DialogHeader>

        <div className="pt-1">
          {loading && <p className="text-gray-500 text-sm py-3">Загрузка...</p>}

          {!loading && (
            <div className="pt-3">
              {statuses.length === 0 ? (
                <p className="text-gray-500 text-sm">Статусы не найдены</p>
              ) : (
                <div className="space-y-1.5">
                  {statuses
                    .slice()
                    .sort((a, b) => a.position - b.position)
                    .map((status) => (
                      <div key={status.id} className="border border-gray-200 rounded-lg px-3 py-2 flex items-center justify-between">
                        <div className="flex items-center gap-2 min-w-0">
                          <span
                            className="h-5 w-5 rounded border border-gray-300 shrink-0"
                            style={{ backgroundColor: status.color ?? '#e5e7eb' }}
                            title={status.color ?? 'Цвет не задан'}
                          />
                          <span className="font-medium text-sm truncate">{status.name}</span>
                        </div>
                        <div className="flex items-center gap-2">
                          <span className="text-gray-500 text-sm">Позиция: {status.position}</span>
                          {statusToDeleteId === status.id ? (
                            <>
                              <button
                                type="button"
                                className="text-xs px-2 py-1 rounded border border-red-600 text-red-600 hover:bg-red-50"
                                onClick={() => handleDeleteStatus(status.id)}
                              >
                                Удалить
                              </button>
                              <button
                                type="button"
                                className="text-xs px-2 py-1 rounded border border-gray-300 text-gray-600 hover:bg-gray-100"
                                onClick={() => setStatusToDeleteId(null)}
                              >
                                Отмена
                              </button>
                            </>
                          ) : (
                            <button
                              type="button"
                              className="text-gray-500 hover:text-red-600"
                              title="Удалить статус"
                              onClick={() => setStatusToDeleteId(status.id)}
                            >
                              🗑️
                            </button>
                          )}
                        </div>
                      </div>
                    ))}
                </div>
              )}

              <div className="mt-2.5">
                <div className="flex gap-2">
                  <input
                    type="color"
                    value={newStatusColor}
                    onChange={(e) => setNewStatusColor(e.target.value)}
                    className="h-10 w-10 cursor-pointer rounded-md border border-gray-300 bg-white p-1"
                    title="Выбрать цвет статуса"
                  />
                  <input
                    type="text"
                    value={newStatusName}
                    onChange={(e) => {
                      setNewStatusName(e.target.value);
                      if (createError) setCreateError(null);
                    }}
                    onKeyDown={(e) => {
                      if (e.key === 'Enter') {
                        e.preventDefault();
                        handleCreateStatus();
                      }
                    }}
                    placeholder="Новый статус"
                    className="flex-1 border border-gray-300 rounded-md px-3 py-2 text-sm"
                  />
                  <button
                    type="button"
                    onClick={handleCreateStatus}
                    disabled={isCreating}
                    className="px-3 py-2 text-sm rounded-md border border-green-600 bg-green-600 text-white hover:bg-green-700 disabled:opacity-60"
                    title="Добавить статус"
                  >
                    {isCreating ? '...' : '+'}
                  </button>
                </div>
                {createError && <p className="text-red-500 text-xs mt-1">{createError}</p>}
                {deleteError && (<p className="text-red-500 text-xs mt-2">{deleteError}</p>)}
              </div>
            </div>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
}
