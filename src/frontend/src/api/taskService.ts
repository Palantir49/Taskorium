import { Task, User, CreateTaskData, UpdateTaskData } from '../types';

/**
 * Сервис для работы с задачами через API
 * В будущем здесь будут реальные запросы к Asp.Net Core Backend
 */

// Моковые данные для задач
const mockTasks: Task[] = [
  {
    id: 1,
    title: "Исправить ошибку авторизации",
    description: "Пользователи не могут войти в систему после обновления. Требуется срочное исправление.",
    status: "backlog",
    priority: "critical",
    type: "bug",
    assignedTo: { id: 1, name: "Иван Петров", initials: "ИП" },
    createdAt: new Date(2025, 10, 5), // Ноябрь 2025
    deadline: new Date(2025, 11, 25), // Декабрь 2025
    subtasks: [
      { id: 1, title: "Проверить логи сервера", completed: true },
      { id: 2, title: "Найти причину ошибки", completed: true },
      { id: 3, title: "Исправить код", completed: false }
    ]
  },
  {
    id: 2,
    title: "Добавить темную тему",
    description: "Реализовать переключение между светлой и темной темой интерфейса.",
    status: "in-progress",
    priority: "high",
    type: "feature",
    assignedTo: { id: 2, name: "Мария Сидорова", initials: "МС" },
    createdAt: new Date(2025, 10, 8), // Ноябрь 2025
    deadline: new Date(2026, 0, 10), // Январь 2026
    subtasks: [
      { id: 4, title: "Создать цветовую палитру", completed: true },
      { id: 5, title: "Реализовать переключатель", completed: false },
      { id: 6, title: "Протестировать на всех страницах", completed: false }
    ]
  },
  {
    id: 3,
    title: "Оптимизировать загрузку данных",
    description: "Улучшить производительность загрузки списка задач при большом количестве данных.",
    status: "in-progress",
    priority: "medium",
    type: "improvement",
    assignedTo: { id: 3, name: "Алексей Иванов", initials: "АИ" },
    createdAt: new Date(2025, 10, 12), // Ноябрь 2025
    deadline: null,
    subtasks: [
      { id: 7, title: "Добавить пагинацию", completed: false },
      { id: 8, title: "Реализовать виртуальный скролл", completed: false }
    ]
  },
  {
    id: 4,
    title: "Написать тесты для API",
    description: "Создать unit и integration тесты для всех endpoints.",
    status: "testing",
    priority: "high",
    type: "improvement",
    assignedTo: { id: 1, name: "Иван Петров", initials: "ИП" },
    createdAt: new Date(2025, 10, 3), // Ноябрь 2025
    deadline: new Date(2026, 0, 5), // Январь 2026
    subtasks: [
      { id: 9, title: "Тесты для GET endpoints", completed: true },
      { id: 10, title: "Тесты для POST endpoints", completed: true },
      { id: 11, title: "Тесты для PUT endpoints", completed: false },
      { id: 12, title: "Тесты для DELETE endpoints", completed: false }
    ]
  },
  {
    id: 5,
    title: "Обновить документацию",
    description: "Актуализировать API документацию с новыми endpoints.",
    status: "testing",
    priority: "low",
    type: "improvement",
    assignedTo: { id: 2, name: "Мария Сидорова", initials: "МС" },
    createdAt: new Date(2025, 11, 15), // Декабрь 2025
    deadline: null,
    subtasks: [
      { id: 13, title: "Обновить Swagger", completed: true },
      { id: 14, title: "Добавить примеры запросов", completed: true }
    ]
  },
  {
    id: 6,
    title: "Исправить баг с отображением дат",
    description: "Даты отображаются в неправильном формате в некоторых браузерах.",
    status: "done",
    priority: "medium",
    type: "bug",
    assignedTo: { id: 3, name: "Алексей Иванов", initials: "АИ" },
    createdAt: new Date(2025, 11, 1), // Декабрь 2025
    deadline: new Date(2025, 11, 20), // Декабрь 2025
    subtasks: [
      { id: 15, title: "Использовать date-fns", completed: true },
      { id: 16, title: "Протестировать в разных браузерах", completed: true }
    ]
  },
  {
    id: 7,
    title: "Добавить уведомления",
    description: "Реализовать систему push-уведомлений для важных событий.",
    status: "backlog",
    priority: "high",
    type: "feature",
    assignedTo: { id: 1, name: "Иван Петров", initials: "ИП" },
    createdAt: new Date(2025, 11, 18), // Декабрь 2025
    deadline: new Date(2026, 0, 15), // Январь 2026
    subtasks: [
      { id: 17, title: "Настроить WebSocket", completed: false },
      { id: 18, title: "Создать компонент уведомлений", completed: false }
    ]
  },
  {
    id: 8,
    title: "Улучшить UI мобильной версии",
    description: "Оптимизировать интерфейс для мобильных устройств.",
    status: "done",
    priority: "medium",
    type: "improvement",
    assignedTo: { id: 2, name: "Мария Сидорова", initials: "МС" },
    createdAt: new Date(2025, 11, 2), // Декабрь 2025
    deadline: new Date(2025, 11, 22), // Декабрь 2025
    subtasks: [
      { id: 19, title: "Адаптивная верстка", completed: true },
      { id: 20, title: "Оптимизация touch-событий", completed: true },
      { id: 21, title: "Тестирование на устройствах", completed: true }
    ]
  },
  {
    id: 9,
    title: "Рефакторинг модуля авторизации",
    description: "Требуется пересмотр архитектуры модуля авторизации. Ожидается решение по выбору библиотеки.",
    status: "pause",
    priority: "high",
    type: "improvement",
    assignedTo: { id: 1, name: "Иван Петров", initials: "ИП" },
    createdAt: new Date(2025, 10, 7), // Ноябрь 2025
    deadline: new Date(2026, 0, 12), // Январь 2026
    subtasks: []
  },
  {
    id: 10,
    title: "Интеграция с платежной системой",
    description: "Интеграция приостановлена до получения необходимых API ключей от партнера.",
    status: "pause",
    priority: "medium",
    type: "feature",
    assignedTo: { id: 3, name: "Алексей Иванов", initials: "АИ" },
    createdAt: new Date(2025, 10, 14), // Ноябрь 2025
    deadline: new Date(2026, 0, 20), // Январь 2026
    subtasks: []
  }
];

// Моковые пользователи
const mockUsers: User[] = [
  { id: 1, name: "Иван Петров", initials: "ИП" },
  { id: 2, name: "Мария Сидорова", initials: "МС" },
  { id: 3, name: "Алексей Иванов", initials: "АИ" }
];

/**
 * Получить все задачи
 */
export const fetchTasks = async (): Promise<Task[]> => {
  // Имитация задержки сети
  await new Promise(resolve => setTimeout(resolve, 300));
  return [...mockTasks];
};

/**
 * Получить задачу по ID
 */
export const fetchTaskById = async (id: number): Promise<Task> => {
  await new Promise(resolve => setTimeout(resolve, 200));
  const task = mockTasks.find(task => task.id === id);
  if (!task) {
    throw new Error('Task not found');
  }
  return task;
};

/**
 * Обновить задачу
 */
export const updateTask = async (id: number, updates: UpdateTaskData): Promise<Task> => {
  await new Promise(resolve => setTimeout(resolve, 300));
  const taskIndex = mockTasks.findIndex(task => task.id === id);
  if (taskIndex === -1) {
    throw new Error('Task not found');
  }
  const updatedTask = { ...mockTasks[taskIndex], ...updates };
  mockTasks[taskIndex] = updatedTask;
  return updatedTask;
};

/**
 * Создать новую задачу
 */
export const createTask = async (taskData: CreateTaskData): Promise<Task> => {
  await new Promise(resolve => setTimeout(resolve, 300));
  const newTask: Task = {
    id: Math.max(...mockTasks.map(t => t.id)) + 1,
    title: taskData.title,
    description: taskData.description,
    status: taskData.status || 'backlog',
    priority: taskData.priority || 'medium',
    type: taskData.type || 'feature',
    assignedTo: taskData.assignedTo || mockUsers[0],
    createdAt: new Date(),
    deadline: taskData.deadline || null,
    subtasks: taskData.subtasks?.map((subtask, index) => ({
      id: Math.max(...mockTasks.flatMap(t => t.subtasks.map(s => s.id)), 0) + index + 1,
      title: subtask.title,
      completed: subtask.completed || false
    })) || []
  };
  mockTasks.push(newTask);
  return newTask;
};

/**
 * Удалить задачу
 */
export const deleteTask = async (id: number): Promise<void> => {
  await new Promise(resolve => setTimeout(resolve, 200));
  const taskIndex = mockTasks.findIndex(task => task.id === id);
  if (taskIndex === -1) {
    throw new Error('Task not found');
  }
  mockTasks.splice(taskIndex, 1);
};

/**
 * Получить всех пользователей
 */
export const fetchUsers = async (): Promise<User[]> => {
  await new Promise(resolve => setTimeout(resolve, 100));
  return [...mockUsers];
};

/**
 * Тестовый контроллер для проверки API
 */
export const fetchTasksTestController = async (): Promise<any> => {
  const response = await fetch('/api/v1/Issues/123e4567-e89b-12d3-a456-426614174000');
  if (!response.ok) {
    throw new Error('Ошибка загрузки задач');
  }
  const data = await response.json();
  console.log('fetchTasksTestController result:', data);
  return data;
};

// Экспорт моковых данных для использования в компонентах
export { mockTasks, mockUsers };
