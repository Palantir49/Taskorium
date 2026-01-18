import React from 'react';
import {
  DndContext,
  DragOverlay,
  closestCorners,
  KeyboardSensor,
  PointerSensor,
  useSensor,
  useSensors
} from '@dnd-kit/core';
import {
  arrayMove,
  sortableKeyboardCoordinates
} from '@dnd-kit/sortable';
import { useTasks } from '../context/TaskContext';
import { filterTasks, groupTasksByStatus } from '../utils/filterTasks';
import Column from './Column';
import TaskCard from './TaskCard';
import { Task, TaskStatus } from '../types';
import './KanbanBoard.css';

const statusOrder: TaskStatus[] = ['backlog', 'in-progress', 'testing', 'pause', 'done'];

interface KanbanBoardProps {
  onCreateTask?: (status: TaskStatus) => void;
}

function KanbanBoard({ onCreateTask }: KanbanBoardProps) {
  console.log('KanbanBoard received onCreateTask:', onCreateTask);
  const { tasks, filters, updateTask, setSelectedTask, selectedTask } = useTasks();
  const [activeTask, setActiveTask] = React.useState<Task | null>(null);

  // Фильтрация задач
  const filteredTasks = React.useMemo(() => {
    return filterTasks(tasks, filters);
  }, [tasks, filters]);

  // Группировка задач по статусам
  const groupedTasks = React.useMemo(() => {
    return groupTasksByStatus(filteredTasks);
  }, [filteredTasks]);

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
    const { active } = event;
    if (active.data.current?.type === 'task') {
      setActiveTask(active.data.current.task);
    }
  };

  // Обработка окончания перетаскивания
  const handleDragEnd = async (event: any) => {
    const { active, over } = event;
    setActiveTask(null);

    if (!over) return;

    const activeTask = active.data.current?.task;
    const overId = over.id;

    if (!activeTask) return;

    // Если задача перетащена в другую колонку
    if (over.data.current?.type === 'column') {
      const newStatus = over.data.current.status;
      if (activeTask.status !== newStatus) {
        try {
          await updateTask(activeTask.id, { status: newStatus });
        } catch (error) {
          console.error('Ошибка при обновлении задачи:', error);
        }
      }
    }
    // Если задача перетащена на другую задачу
    else if (over.data.current?.type === 'task') {
      const overTask = over.data.current.task;
      if (activeTask.status !== overTask.status) {
        try {
          await updateTask(activeTask.id, { status: overTask.status });
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

  return (
    <div className="kanban-board">
      <DndContext
        sensors={sensors}
        collisionDetection={closestCorners}
        onDragStart={handleDragStart}
        onDragEnd={handleDragEnd}
      >
        <div className={`kanban-board-content ${selectedTask ? 'sidebar-open' : ''}`}>
          {statusOrder.map(status => (
            <Column
              key={status}
              status={status}
              tasks={groupedTasks[status] || []}
              onTaskClick={handleTaskClick}
              isSidebarOpen={!!selectedTask}
              onCreateTask={onCreateTask}
            />
          ))}
        </div>
        <DragOverlay>
          {activeTask ? (
            <div style={{ opacity: 0.8, transform: 'rotate(5deg)' }}>
              <TaskCard task={activeTask} onClick={() => {}} />
            </div>
          ) : null}
        </DragOverlay>
      </DndContext>
    </div>
  );
}

export default KanbanBoard;
