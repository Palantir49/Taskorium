import { ProjectResponse, CreateProjectRequest, UpdateProjectRequest, ProjectMembersResponse } from '../types/project';
import { RoleDto } from '../types/common';
import { IssueResponse } from '../types/issue';
import { IssueStatusResponse } from '../types/issueStatus';
import { TagResponse } from '../types/tag';
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
 * Сервис для работы с проектами
 */

/**
 * Получить проект по ID
 */
export const fetchProjectById = async (id: string): Promise<ProjectResponse> => {
  try {
    const response = await api.get(`/Projects/${id}`);
    console.log('fetchProjectById:', response.data);
    return response.data;
  } catch (error) {
    console.error('fetchProjectById error:', error);
    throw error;
  }
};

/**
 * Создать новый проект
 */
export const createProject = async (projectData: CreateProjectRequest): Promise<ProjectResponse> => {
  console.log('createProject called:', projectData);
  try {
    const { workspaceId, ...body } = projectData;
    const response = await api.post(`/Workspace/${workspaceId}/project`, body);
    console.log('createProject:', response.data);
    return response.data;
  } catch (error) {
    console.error('createProject error:', error);
    throw error;
  }
};

/**
 * Обновить проект
 */
export const updateProject = async (id: string, updates: UpdateProjectRequest): Promise<ProjectResponse> => {
  console.log('updateProject called:', id, 'updates:', updates);
  try {
    const response = await api.put(`/Projects/${id}`, updates);
    console.log('updateProject:', response.data);
    return response.data;
  } catch (error) {
    console.error('updateProject error:', error);
    throw error;
  }
};

/**
 * Удалить проект
 */
export const deleteProject = async (id: string): Promise<void> => {
  console.log('deleteProject:', id);
  try {
    const response = await api.delete(`/Projects/${id}`);
    console.log('deleteProject:', response.status);
  } catch (error) {
    console.error('deleteProject error:', error);
    throw error;
  }
};

/**
 * Добавить пользователя в проект
 */
export interface AddProjectMemberRequest {
  userId: string;
  roleDto: RoleDto;
}

export const addUserToProject = async (projectId: string, command: AddProjectMemberRequest): Promise<ProjectResponse> => {
  console.log('addUserToProject called:', projectId, command);
  try {
    const response = await api.post(`/Projects/${projectId}/projectMember`, command);
    console.log('addUserToProject:', response.data);
    return response.data;
  } catch (error) {
    console.error('addUserToProject error:', error);
    throw error;
  }
};

/**
 * Получить все задачи проекта
 */
export const fetchIssuesByProjectId = async (id: string): Promise<IssueResponse[]> => {
  try {
    const response = await api.get(`/Projects/${id}/Issues`);
    console.log('fetchIssuesByProjectId:', response.data);
    return response.data || [];
  } catch (error) {
    console.error('fetchIssuesByProjectId error:', error);
    return [];
  }
};

/**
 * Получить все статусы задачи проекта
 */
export const fetchIssueStatusesByProjectId = async (id: string): Promise<IssueStatusResponse[]> => {
  try {
    const response = await api.get(`/Projects/${id}/IssueStatuses`);
    console.log('fetchIssueStatusesByProjectId:', response.data);
    return response.data || [];
  } catch (error) {
    console.error('fetchIssueStatusesByProjectId error:', error);
    return [];
  }
};

/**
 * Получить все теги проекта
 */
export const fetchTagsByProjectId = async (id: string): Promise<TagResponse[]> => {
  try {
    const response = await api.get(`/Projects/${id}/Tags`);
    console.log('fetchTagsByProjectId:', response.data);
    return response.data || [];
  } catch (error) {
    console.error('fetchTagsByProjectId error:', error);
    return [];
  }
};

/**
 * Получить участников проекта
 */
export const fetchProjectMembers = async (projectId: string): Promise<ProjectMembersResponse> => {
  try {
    const response = await api.get(`/Projects/${projectId}/members`);
    console.log('fetchProjectMembers:', response.data);
    return response.data;
  } catch (error) {
    console.error('fetchProjectMembers error:', error);
    throw error;
  }
};
