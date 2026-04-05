import { Task, User, CreateTaskData, UpdateTaskData } from '../types';

/**
 * Сервис для работы с задачами через API
 * В будущем здесь будут реальные запросы к Asp.Net Core Backend
 */

// Моковые данные для задач (совместимые с DTO)
const mockTasks: Task[] = [
  {
    id: "1",
    name: "Исправить ошибку авторизации",
    description: "Пользователи не могут войти в систему после обновления. Требуется срочное исправление.",
    projectId: "1",
    taskStatusId: "1",
    issueType: { number: 1, name: "bug", displayName: "Баг", code: "BUG" },
    issuePriority: { number: 4, name: "critical", displayName: "Критический" },
    createdDate: "2025-11-05T00:00:00",
    updatedDate: "2025-11-05T00:00:00",
    dueDate: "2025-12-25T00:00:00",
    resolvedDate: null
  },
  {
    id: "2",
    name: "Добавить темную тему",
    description: "Реализовать переключение между светлой и темной темой интерфейса.",
    projectId: "1",
    taskStatusId: "2",
    issueType: { number: 2, name: "feature", displayName: "Функция", code: "FEATURE" },
    issuePriority: { number: 3, name: "high", displayName: "Высокий" },
    createdDate: "2025-11-08T00:00:00",
    updatedDate: "2025-11-08T00:00:00",
    dueDate: "2026-01-10T00:00:00",
    resolvedDate: null
  },
  {
    id: "3",
    name: "Оптимизировать загрузку данных",
    description: "Улучшить производительность загрузки списка задач при большом количестве данных.",
    projectId: "1",
    taskStatusId: "2",
    issueType: { number: 3, name: "improvement", displayName: "Улучшение", code: "IMPROVEMENT" },
    issuePriority: { number: 2, name: "medium", displayName: "Средний" },
    createdDate: "2025-11-12T00:00:00",
    updatedDate: "2025-11-12T00:00:00",
    dueDate: null,
    resolvedDate: null
  },
  {
    id: "4",
    name: "Написать тесты для API",
    description: "Создать unit и integration тесты для всех endpoints.",
    projectId: "1",
    taskStatusId: "3",
    issueType: { number: 3, name: "improvement", displayName: "Улучшение", code: "IMPROVEMENT" },
    issuePriority: { number: 3, name: "high", displayName: "Высокий" },
    createdDate: "2025-11-03T00:00:00",
    updatedDate: "2025-11-03T00:00:00",
    dueDate: "2026-01-05T00:00:00",
    resolvedDate: null
  },
  {
    id: "5",
    name: "Обновить документацию",
    description: "Актуализировать API документацию с новыми endpoints.",
    projectId: "1",
    taskStatusId: "3",
    issueType: { number: 3, name: "improvement", displayName: "Улучшение", code: "IMPROVEMENT" },
    issuePriority: { number: 1, name: "low", displayName: "Низкий" },
    createdDate: "2025-12-15T00:00:00",
    updatedDate: "2025-12-15T00:00:00",
    dueDate: null,
    resolvedDate: null
  },
  {
    id: "6",
    name: "Исправить баг с отображением дат",
    description: "Даты отображаются в неправильном формате в некоторых браузерах.",
    projectId: "1",
    taskStatusId: "4",
    issueType: { number: 1, name: "bug", displayName: "Баг", code: "BUG" },
    issuePriority: { number: 2, name: "medium", displayName: "Средний" },
    createdDate: "2025-12-01T00:00:00",
    updatedDate: "2025-12-01T00:00:00",
    dueDate: "2025-12-20T00:00:00",
    resolvedDate: "2025-12-20T00:00:00"
  },
  {
    id: "7",
    name: "Добавить уведомления",
    description: "Реализовать систему push-уведомлений для важных событий.",
    projectId: "1",
    taskStatusId: "1",
    issueType: { number: 2, name: "feature", displayName: "Функция", code: "FEATURE" },
    issuePriority: { number: 3, name: "high", displayName: "Высокий" },
    createdDate: "2025-12-18T00:00:00",
    updatedDate: "2025-12-18T00:00:00",
    dueDate: "2026-01-15T00:00:00",
    resolvedDate: null
  },
  {
    id: "8",
    name: "Улучшить UI мобильной версии",
    description: "Оптимизировать интерфейс для мобильных устройств.",
    projectId: "1",
    taskStatusId: "4",
    issueType: { number: 3, name: "improvement", displayName: "Улучшение", code: "IMPROVEMENT" },
    issuePriority: { number: 2, name: "medium", displayName: "Средний" },
    createdDate: "2025-12-02T00:00:00",
    updatedDate: "2025-12-02T00:00:00",
    dueDate: "2025-12-22T00:00:00",
    resolvedDate: "2025-12-22T00:00:00"
  },
  {
    id: "9",
    name: "Рефакторинг модуля авторизации",
    description: "Требуется пересмотр архитектуры модуля авторизации. Ожидается решение по выбору библиотеки.",
    projectId: "1",
    taskStatusId: "5",
    issueType: { number: 3, name: "improvement", displayName: "Улучшение", code: "IMPROVEMENT" },
    issuePriority: { number: 3, name: "high", displayName: "Высокий" },
    createdDate: "2025-11-07T00:00:00",
    updatedDate: "2025-11-07T00:00:00",
    dueDate: "2026-01-12T00:00:00",
    resolvedDate: null
  },
  {
    id: "10",
    name: "Интеграция с платежной системой",
    description: "Интеграция приостановлена до получения необходимых API ключей от партнера.",
    projectId: "1",
    taskStatusId: "5",
    issueType: { number: 2, name: "feature", displayName: "Функция", code: "FEATURE" },
    issuePriority: { number: 2, name: "medium", displayName: "Средний" },
    createdDate: "2025-11-14T00:00:00",
    updatedDate: "2025-11-14T00:00:00",
    dueDate: "2026-01-20T00:00:00",
    resolvedDate: null
  }
];

// Моковые пользователи (совместимые с DTO)
const mockUsers: User[] = [
  { id: "1", keycloakId: "1", email: "ivan.petrov@example.com", username: "ivan.petrov", createdAt: "2025-01-01T00:00:00" },
  { id: "2", keycloakId: "2", email: "maria.sidorova@example.com", username: "maria.sidorova", createdAt: "2025-01-02T00:00:00" },
  { id: "3", keycloakId: "3", email: "alexey.ivanov@example.com", username: "alexey.ivanov", createdAt: "2025-01-03T00:00:00" }
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
export const fetchTaskById = async (id: string): Promise<Task> => {
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
export const updateTask = async (id: string, updates: UpdateTaskData): Promise<Task> => {
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
    id: (mockTasks.length + 1).toString(),
    name: taskData.name,
    description: taskData.description,
    projectId: taskData.projectId,
    taskStatusId: "1", // По умолчанию в бэклоге
    issueType: { number: taskData.numberIssueType, name: "", displayName: "", code: "" },
    issuePriority: { number: taskData.numberIssuePriority, name: "", displayName: "" },
    createdDate: new Date().toISOString(),
    updatedDate: new Date().toISOString(),
    dueDate: taskData.dueDate || null,
    resolvedDate: null
  };
  mockTasks.push(newTask);
  return newTask;
};

/**
 * Удалить задачу
 */
export const deleteTask = async (id: string): Promise<void> => {
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
 /* const response = await fetch('/api/v1/Issues/123e4567-e89b-12d3-a456-426614174000');
  if (!response.ok) {
    throw new Error('Ошибка загрузки задач');
  }
  const data = await response.json();
  console.log('fetchTasksTestController result:', data);
  return data;*/
};

// Экспорт моковых данных для использования в компонентах
export { mockTasks, mockUsers };