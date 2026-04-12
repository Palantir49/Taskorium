import { Task, User, CreateTaskData, UpdateTaskData } from '../types';
import axios from 'axios';

// Функция для получения токена доступа из react-oidc-context
// Нужно вызывать только из React компонента или хуков
let tokenProvider: () => string | null = () => null;

export const setTokenProvider = (provider: () => string | null) => {
  tokenProvider = provider;
};

// Настройка axios
const api = axios.create({
  baseURL: '/api/v1',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Интерсептор для добавления JWT-токена
api.interceptors.request.use((config) => {
  const token = tokenProvider();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  console.log('API Request:', config.method?.toUpperCase(), config.url, config.data);
  return config;
});

// Интерсептор для обработки ошибок
api.interceptors.response.use(
  (response) => {
    console.log('API Response:', response.status, response.config.method?.toUpperCase(), response.config.url, response.data);
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.status, error.config?.method?.toUpperCase(), error.config?.url, error.response?.data);
    if (error.response?.status === 401) {
      console.log('401 Unauthorized');
    }
    return Promise.reject(error);
  }
);

/**
 * Сервис для работы с задачами через API
 */

/**
 * Получить все задачи
 */
export const fetchTasks = async (): Promise<Task[]> => {
  console.log('fetchTasks called');
  try {
    const response = await api.get('/Issues');
    console.log('fetchTasks:', response.data);
    return response.data.issues || [];
  } catch (error) {
    console.error('fetchTasks error:', error);
    return [];
  }
};

/**
 * Получить задачу по ID
 */
export const fetchTaskById = async (id: string): Promise<Task> => {
  try {
    const response = await api.get(`/Issues/${id}`);
    console.log('fetchTaskById:', response.data);
    return response.data;
  } catch (error) {
    console.error('fetchTaskById error:', error);
    throw error;
  }
};

/**
 * Обновить задачу
 */
export const updateTask = async (id: string, updates: UpdateTaskData): Promise<Task> => {
  console.log('updateTask called:', id, 'updates:', updates);
  try {
    const response = await api.put(`/Issues/${id}`, updates);
    console.log('updateTask:', response.data);
    return response.data;
  } catch (error) {
    console.error('updateTask error:', error);
    throw error;
  }
};

/**
 * Создать новую задачу
 */
export const createTask = async (taskData: CreateTaskData): Promise<Task> => {
  console.log('createTask called:', taskData);
  try {
    const response = await api.post('/Issues', taskData);
    console.log('createTask:', response.data);
    return response.data;
  } catch (error) {
    console.error('createTask error:', error);
    throw error;
  }
};

/**
 * Удалить задачу
 */
export const deleteTask = async (id: string): Promise<void> => {
  console.log('deleteTask:', id);
  try {
    const response = await api.delete(`/Issues/${id}`);
    console.log('deleteTask:', response.status);
  } catch (error) {
    console.error('deleteTask error:', error);
    throw error;
  }
};

/**
 * Получить всех пользователей
 */
export const fetchUsers = async (): Promise<User[]> => {
  console.log('fetchUsers called');
  try {
    const response = await api.get('/User/GetAllUsers');
    console.log('fetchUsers response data:', response.data);
    
    // Возвращает объект с массивом пользователей в свойстве "users"
    if (response.data && Array.isArray(response.data.users)) {
      return response.data.users;
    } else {
      console.warn('Unexpected users response format:', response.data);
      return [];
    }
  } catch (error) {
    console.error('fetchUsers error:', error);
    return [];
  }
};

/**
 * Тестовый контроллер для проверки API
 */
export const fetchTasksTestController = async (): Promise<any> => {
  try {
    const response = await api.get('/Issues');
    console.log('API test result:', response.data);
    return response.data;
  } catch (error) {
    console.error('API test error:', error);
    return null;
  }
};