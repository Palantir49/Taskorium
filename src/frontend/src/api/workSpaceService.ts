import {
    AddUserToWorkspaceRequest,
    CreateWorkspaceRequest,
    UpdateWorkspaceRequest,
    WorkspaceMembersResponse,
    WorkspaceResponse
} from '../types';
import {api} from "./axios.ts";


/**
 * Сервис для работы с рабочими пространствами
 */

/**
 * Получить рабочую область по ID
 */
export const fetchWorkspaceById = async (id: string): Promise<WorkspaceResponse> => {
    try {
        const response = await api.get(`/Workspace/${id}`);
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
export const fetchWorkspacesPage = async (params?: Record<string, string | number>): Promise<unknown> => {
    try {
        const response = await api.get('/Workspace/page', {params});
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
        const response = await api.post('/Workspace', workspaceData);
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
export const updateWorkspaceName = async (workspaceId: string, command: UpdateWorkspaceRequest): Promise<WorkspaceResponse> => {
    console.log('updateWorkspaceName called:', workspaceId, command);
    try {
        const response = await api.patch(`/Workspace/${workspaceId}`, command);
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
export const addUserToWorkspace = async (workspaceId: string, command: AddUserToWorkspaceRequest): Promise<unknown> => {
    console.log('addUserToWorkspace called:', workspaceId, command);
    try {
        const response = await api.post(`/Workspace/${workspaceId}/users`, command);
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
        const response = await api.get('/Workspace/page');
        console.log('fetchUserWorkspaces:', response.data);
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
        const response = await api.delete(`/Workspace/${id}`);
        console.log('deleteWorkspace:', response.status);
    } catch (error) {
        console.error('deleteWorkspace error:', error);
        throw error;
    }
};


export const fetchWorkspaceMembers = async (id: string): Promise<WorkspaceMembersResponse> => {
    console.log('fetchWorkspaceMembers:', id);
    try {
        const response = await api.get(`Workspace/${id}/members`);
        console.log('fetchWorkspaceMembers:', response.data);
        return response.data;
    } catch (error) {
        console.error('fetchWorkspaceMembers error:', error);
        throw error;
    }
}



