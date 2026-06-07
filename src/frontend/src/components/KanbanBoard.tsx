import React from 'react';
import {
    closestCorners,
    DndContext,
    DragOverlay,
    KeyboardSensor,
    PointerSensor,
    useSensor,
    useSensors
} from '@dnd-kit/core';
import {sortableKeyboardCoordinates} from '@dnd-kit/sortable';
import {useTasks} from '../context/TaskContext';
import {fetchIssueStatusesByProjectId} from '../api/projectService';
import {filterTasks} from '../utils/filterTasks';
import Column from './Column';
import TaskCard from './TaskCard';
import {Task} from '../types';
import {IssueStatusResponse} from '../types/issueStatus';
import './KanbanBoard.css';

interface KanbanBoardProps {
    projectId: string;
}

const COLOR_STATUS = [
    'gray', 'lightblue', 'yellow', 'palegreen', 'pink',
    'lavander', 'green'
];

const getStableColorById = (position: number): string => {
    return COLOR_STATUS[position];
};

function KanbanBoard({projectId}: KanbanBoardProps) {
    const {tasks, filters, updateTask, setSelectedTask, selectedTask} = useTasks();
    const [activeTask, setActiveTask] = React.useState<Task | null>(null);
    const [statuses, setStatuses] = React.useState<IssueStatusResponse[]>([]);
    const [loadingStatuses, setLoadingStatuses] = React.useState(false);

    React.useEffect(() => {
        const loadStatuses = async () => {
            setLoadingStatuses(true);
            try {
                const data = await fetchIssueStatusesByProjectId(projectId);
                const sorted = [...(data ?? [])].sort((a, b) => a.position - b.position);
                setStatuses(sorted);
            } catch (error) {
                console.error('Ошибка загрузки статусов проекта:', error);
                setStatuses([]);
            } finally {
                setLoadingStatuses(false);
            }
        };

        loadStatuses();
    }, [projectId]);

    // Фильтрация задач
    const filteredTasks = React.useMemo(() => {
        return filterTasks(tasks, filters);
    }, [tasks, filters]);

    // Группировка задач по статусам проекта
    const groupedTasks = React.useMemo(() => {
        const groups: Record<string, Task[]> = {};
        statuses.forEach((status) => {
            groups[status.id] = [];
        });

        filteredTasks.forEach((task) => {
            if (groups[task.taskStatusId]) {
                groups[task.taskStatusId].push(task);
            }
            // задачи с неизвестным статусом скрываем
        });

        return groups;
    }, [filteredTasks, statuses]);

    const getUpdatePayload = (task: Task, issueStatusId: string) => ({
        name: task.name,
        issueStatusId,
        numberIssueType: task.issueType.number,
        numberIssuePriority: task.issuePriority.number,
        description: task.description,
        dueDate: task.dueDate ?? null,
        assignees: task.assignees

    });

    // Настройка сенсоров для drag-and-drop
    const sensors = useSensors(
        useSensor(PointerSensor, {
            activationConstraint: {
                distance: 8
            }
        }),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates
        })
    );

    // Обработка начала перетаскивания
    const handleDragStart = (event: any) => {
        const {active} = event;
        if (active.data.current?.type === 'task') {
            setActiveTask(active.data.current.task);
        }
    };

    // Обработка окончания перетаскивания
    const handleDragEnd = async (event: any) => {
        const {active, over} = event;
        setActiveTask(null);

        if (!over) return;

        const activeTask = active.data.current?.task;
        if (!activeTask) return;

        // Если задача перенесена в другую колонку
        if (over.data.current?.type === 'column') {
            const newStatusId = over.data.current.statusId;
            if (activeTask.taskStatusId !== newStatusId) {
                try {
                    await updateTask(activeTask.id, getUpdatePayload(activeTask, newStatusId));
                } catch (error) {
                    console.error('Ошибка при обновлении задачи:', error);
                }
            }
        }
        // Если задача перенесена в другую колонку
        else if (over.data.current?.type === 'task') {
            const overTask = over.data.current.task;
            if (activeTask.taskStatusId !== overTask.taskStatusId) {
                try {
                    await updateTask(activeTask.id, getUpdatePayload(activeTask, overTask.taskStatusId));
                } catch (error) {
                    console.error('Ошибка при обновлении задачи:', error);
                }
            }
        }
    };

    // Обработка клика на карточку задачи
    const handleTaskClick = (task: Task) => {
        setSelectedTask(task);
    };

    if (loadingStatuses) {
        return (
            <div className="kanban-board">
                <p className="text-gray-500 text-sm">Загрузка статусов проекта...</p>
            </div>
        );
    }

    return (
        <div className="kanban-board">
            <DndContext
                sensors={sensors}
                collisionDetection={closestCorners}
                onDragStart={handleDragStart}
                onDragEnd={handleDragEnd}
            >
                <div className={`kanban-board-content ${selectedTask ? 'sidebar-open' : ''}`}>
                    {statuses.map((status, index) => (
                        <Column
                            key={status.id}
                            statusId={status.id}
                            title={status.name}
                            color={getStableColorById(status.position)}
                            tasks={groupedTasks[status.id] || []}
                            onTaskClick={handleTaskClick}
                            isSidebarOpen={!!selectedTask}
                        />
                    ))}
                </div>
                <DragOverlay>
                    {activeTask ? (
                        <div style={{opacity: 0.8, transform: 'rotate(5deg)'}}>
                            <TaskCard task={activeTask} onClick={() => {
                            }}/>
                        </div>
                    ) : null}
                </DragOverlay>
            </DndContext>
        </div>
    );
}

export default KanbanBoard;