import { Task, TaskFilters } from '../types';

/**
 * Утилита для фильтрации задач
 * Применяет все активные фильтры к списку задач
 */

/**
 * Фильтрует задачи по заданным критериям
 */
export function filterTasks(tasks: Task[], filters: TaskFilters): Task[] {
  return tasks.filter(task => {
    // Фильтр по исполнителю
    if (filters.assignedTo && task.assignedTo?.id !== parseInt(filters.assignedTo)) {
      return false;
    }

    // Фильтр по типу задачи
    if (filters.type && task.type !== filters.type) {
      return false;
    }

    // Фильтр по важности
    if (filters.priority && task.priority !== filters.priority) {
      return false;
    }

    // Фильтр по дате создания
    if (filters.createdAt) {
      const now = new Date();
      const taskDate = new Date(task.createdAt);

      switch (filters.createdAt) {
        case 'today':
          const todayStart = new Date(now.getFullYear(), now.getMonth(), now.getDate());
          if (taskDate < todayStart) return false;
          break;
        case 'week':
          const weekAgo = new Date(now);
          weekAgo.setDate(weekAgo.getDate() - 7);
          if (taskDate < weekAgo) return false;
          break;
        case 'month':
          const monthAgo = new Date(now);
          monthAgo.setMonth(monthAgo.getMonth() - 1);
          if (taskDate < monthAgo) return false;
          break;
        case 'all':
          // Пропускаем фильтр
          break;
        default:
          break;
      }
    }

    // Фильтр по дедлайну
    if (filters.deadline) {
      const now = new Date();
      now.setHours(0, 0, 0, 0);

      switch (filters.deadline) {
         // Показываем только задачи с просроченным дедлайном, которые еще не выполнены
        case 'overdue':
          if (!task.deadline) return true;
          // Исключаем выполненные задачи
          if (task.status === 'done') return false;
          const overdueDeadline = new Date(task.deadline);
          overdueDeadline.setHours(0, 0, 0, 0);
          // Дедлайн должен быть меньше сегодняшней даты (просрочен)
          if (overdueDeadline >= now) return false;
          break;

        case 'this-week':
          // Показываем задачи с дедлайном на этой неделе (сегодня + 7 дней)
          if (!task.deadline) return false;
          const weekDeadline = new Date(task.deadline);
          weekDeadline.setHours(0, 0, 0, 0);
          const weekEnd = new Date(now);
          weekEnd.setDate(weekEnd.getDate() + 7);
          if (weekDeadline < now || weekDeadline > weekEnd) return false;
          break;

        case 'this-month':
          // Показываем задачи с дедлайном в этом месяце (от сегодня до конца месяца)
          if (!task.deadline) return false;
          const monthDeadline = new Date(task.deadline);
          monthDeadline.setHours(0, 0, 0, 0);
          const monthEnd = new Date(now.getFullYear(), now.getMonth() + 1, 0);
          monthEnd.setHours(23, 59, 59, 999);
          // Дедлайн должен быть >= сегодня и <= последний день месяца
          if (monthDeadline < now || monthDeadline > monthEnd) return false;
          break;

        case 'no-deadline':
          // Показываем только задачи без дедлайна
          if (task.deadline) return false;
          break;

        default:
          break;
      }
    }

    return true;
  });
}

/**
 * Группирует задачи по статусам
 */
export function groupTasksByStatus(tasks: Task[]): Record<string, Task[]> {
  const statuses: Record<string, Task[]> = {
    backlog: [],
    'in-progress': [],
    testing: [],
    pause: [],
    done: []
  };

  tasks.forEach(task => {
    if (statuses[task.status]) {
      statuses[task.status].push(task);
    }
  });

  return statuses;
}
