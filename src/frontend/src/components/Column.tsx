import React from 'react';
import { useDroppable } from '@dnd-kit/core';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import TaskCard from './TaskCard';
import { Task } from '../types';
import './Column.css';

interface ColumnProps {
  statusId: string;
  title: string;
  color: string;
  tasks: Task[];
  onTaskClick?: (task: Task) => void;
  isSidebarOpen?: boolean;
}

function Column({ statusId, title, color, tasks, onTaskClick, isSidebarOpen }: ColumnProps) {

  const { setNodeRef, isOver } = useDroppable({
    id: statusId,
    data: {
      type: 'column',
      statusId
    }
  });

  return (
    <>
      <div
        ref={setNodeRef}
        className={`column ${isOver ? 'column-over' : ''} ${isSidebarOpen ? 'sidebar-open' : ''}`}
      >
        <div className="column-header" style={{ backgroundColor: color }}>
          <h2 className="column-title">
            {title}
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
        </div>
      </div>
      <div className="column-divider"></div>
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
