import {CreateProjectRequest, ProjectMembersResponse, ProjectResponse, UpdateProjectRequest} from '../types/project';
import {IssueResponse, IssueStatusResponse, TagResponse} from '../types';
import {api} from "./axios.ts";

// Функция для получения токена доступа из react-oidc-context
// Нужно вызывать только из React компонента или хуков


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
 *  Получить все проекты рабочей области
 */
export const fetchProjectsByWorkspaceId = async (workspaceId: string): Promise<ProjectResponse[]> => {
    try {
        const response = await api.get(`/Projects/workspace/${workspaceId}`);
        console.log('fetchProjectsByWorkspaceId:', response.data);
        return response.data || [];
    } catch (error) {
        console.error('fetchProjectsByWorkspaceId error:', error);
        return [];
    }
};

/**
 * Создать новый проект
 */
export const createProject = async (projectData: CreateProjectRequest): Promise<ProjectResponse> => {
    console.log('createProject called:', projectData);
    try {
        const {workspaceId, ...body} = projectData;
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
    roleDto: number;
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
