import React from 'react';
import { useDroppable } from '@dnd-kit/core';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import TaskCard from './TaskCard';
import { ColumnProps, Task, TaskStatus } from '../types';
import './Column.css';

const columnConfig: Record<TaskStatus, { title: string; color: string; id: string }> = {
  backlog: {
    title: 'Бэклог',
    color: 'var(--column-backlog)',
    id: 'backlog'
  },
  'in-progress': {
    title: 'В работе',
    color: 'var(--column-in-progress)',
    id: 'in-progress'
  },
  testing: {
    title: 'В тестировании',
    color: 'var(--column-testing)',
    id: 'testing'
  },
  pause: {
    title: 'Пауза',
    color: 'var(--column-pause)',
    id: 'pause'
  },
  done: {
    title: 'Готово',
    color: 'var(--column-done)',
    id: 'done'
  }
};

function Column({ status, tasks, onTaskClick, isSidebarOpen, onCreateTask }: ColumnProps) {
  const config = columnConfig[status];

  const { setNodeRef, isOver } = useDroppable({
    id: status,
    data: {
      type: 'column',
      status
    }
  });

  const handleAddTask = () => {
    console.log('Button + clicked in column:', status);
    console.log('onCreateTask function:', onCreateTask);
    if (onCreateTask) {
      console.log('Calling onCreateTask with status:', status);
      onCreateTask(status);
    } else {
      console.log('onCreateTask is undefined!');
    }
  };

  return (
    <>
      <div
        ref={setNodeRef}
        className={`column ${isOver ? 'column-over' : ''} ${isSidebarOpen ? 'sidebar-open' : ''}`}
      >
        <div className="column-header" style={{ backgroundColor: config.color }}>
          <h2 className="column-title">
            {config.title}
            <span className="column-count">({tasks.length})</span>
          </h2>
        </div>
        <div className="column-content">
          {tasks.length === 0 ? (
            <div className="column-empty">Нет задач</div>
          ) : (
            tasks.map(task => (
              <SortableTaskCard
                key={task.id}
                task={task}
                onTaskClick={onTaskClick}
              />
            ))
          )}
          <button className="add-task-btn" onClick={handleAddTask} title="Добавить задачу">
            +
          </button>
        </div>
      </div>
      {status !== 'done' && <div className="column-divider"></div>}
    </>
  );
}

// Отдельный компонент для перетаскиваемой карточки задачи
function SortableTaskCard({ task, onTaskClick }: { task: Task; onTaskClick?: (task: Task) => void }) {
  const {
    setNodeRef,
    attributes,
    listeners,
    transform,
    transition,
    isDragging
  } = useSortable({
    id: task.id,
    data: {
      type: 'task',
      task
    }
  });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1
  };

  return (
    <div ref={setNodeRef} style={style} {...attributes} {...listeners}>
      <TaskCard task={task} onClick={onTaskClick} />
    </div>
  );
}

export default Column;
