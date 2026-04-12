import { WorkspaceResponse, CreateWorkspaceRequest } from '../types/workspace';
import axios from 'axios';

let tokenProvider: () => string | null = () => null;

export const setTokenProvider = (provider: () => string | null) => {
  tokenProvider = provider;
};

const api = axios.create({
  baseURL: '/api/v1',
  headers: {
    'Content-Type': 'application/json',
  },
});

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
 * Сервис для работы с рабочими пространствами
 */

/**
 * Получить рабочую область по ID
 */
export const fetchWorkspaceById = async (id: string): Promise<WorkspaceResponse> => {
  try {
    const response = await api.get(`/WorkSpaces/${id}`);
    console.log('fetchWorkspaceById:', response.data);
    return response.data;
  } catch (error) {
    console.error('fetchWorkspaceById error:', error);
    throw error;
  }
};

/**
 * Получить страницу рабочих областей
 * (с пагинацией)
 */
export const fetchWorkspacesPage = async (params?: any): Promise<any> => {
  try {
    const response = await api.get('/WorkSpaces/GetWorkspacePage', { params });
    console.log('fetchWorkspacesPage:', response.data);
    return response.data;
  } catch (error) {
    console.error('fetchWorkspacesPage error:', error);
    return null;
  }
};

/**
 * Создать новую рабочую область
 */
export const createWorkspace = async (workspaceData: CreateWorkspaceRequest): Promise<WorkspaceResponse> => {
  console.log('createWorkspace called:', workspaceData);
  try {
    const response = await api.post('/WorkSpaces', workspaceData);
    console.log('createWorkspace:', response.data);
    return response.data;
  } catch (error) {
    console.error('createWorkspace error:', error);
    throw error;
  }
};

/**
 * Обновить название рабочей области
 */
export const updateWorkspaceName = async (command: any): Promise<WorkspaceResponse> => {
  console.log('updateWorkspaceName called:', command);
  try {
    const response = await api.patch('/WorkSpaces', command);
    console.log('updateWorkspaceName:', response.data);
    return response.data;
  } catch (error) {
    console.error('updateWorkspaceName error:', error);
    throw error;
  }
};

/**
 * Добавить пользователя в рабочую область
 */
export const addUserToWorkspace = async (command: any): Promise<any> => {
  console.log('addUserToWorkspace called:', command);
  try {
    const response = await api.post('/WorkSpaces/adduser', command);
    console.log('addUserToWorkspace:', response.data);
    return response.data;
  } catch (error) {
    console.error('addUserToWorkspace error:', error);
    throw error;
  }
};

/**
 * Получить все рабочие области пользователя
 * (без пагинации, для отображения на главной странице)
 */
export const fetchUserWorkspaces = async (): Promise<WorkspaceResponse[]> => {
  try {
    const response = await api.get('/WorkSpaces/GetWorkspacePage');
    console.log('fetchUserWorkspaces:', response.data);
    
    // Предполагаем, что бэкенд возвращает список рабочих областей в свойстве "workspaces" или напрямую
    if (response.data && Array.isArray(response.data.workspaces)) {
      return response.data.workspaces;
    } else if (response.data && Array.isArray(response.data)) {
      return response.data;
    } else {
      console.warn('Unexpected workspaces response format:', response.data);
      return [];
    }
  } catch (error) {
    console.error('fetchUserWorkspaces error:', error);
    return [];
  }
};

/**
 * Удалить рабочую область
 */
export const deleteWorkspace = async (id: string): Promise<void> => {
  console.log('deleteWorkspace:', id);
  try {
    const response = await api.delete(`/WorkSpaces/${id}`);
    console.log('deleteWorkspace:', response.status);
  } catch (error) {
    console.error('deleteWorkspace error:', error);
    throw error;
  }
};
