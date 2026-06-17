import {CreateTaskData, IssueUpdateStatusRequest, Task, UpdateTaskData, User} from '../types';
import {api} from "./axios.ts";


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
 * Обновить только статус задачи через PATCH /Issues/{id}
 * Соответствует backend IssuesController.UpdateIssueStatusAsync + IssueUpdateStatusRequest.
 */
export const updateTaskStatus = async (id: string, request: IssueUpdateStatusRequest): Promise<Task> => {
    console.log('updateTaskStatus called:', id, 'request:', request);
    try {
        const response = await api.patch(`/Issues/${id}`, request);
        console.log('updateTaskStatus:', response.data);
        return response.data;
    } catch (error) {
        console.error('updateTaskStatus error:', error);
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
 * Создать новую задачу в проекте (DTO + attachments через multipart/form-data)
 */
export const createTaskInProject = async (
    workspaceId: string,
    projectId: string,
    taskFormData: FormData
): Promise<Task> => {
    console.log('createTaskInProject called:', workspaceId, projectId);
    try {
        const response = await api.post(
            `/Workspace/${workspaceId}/project/${projectId}/issue`,
            taskFormData,
            {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        );
        console.log('createTaskInProject:', response.data);
        return response.data;
    } catch (error) {
        console.error('createTaskInProject error:', error);
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
        const response = await api.get('/User/users');
        console.log('fetchUsers response data:', response.data);

        if (response.data && Array.isArray(response.data.users)) {
            return response.data.users;
        } else if (response.data && Array.isArray(response.data)) {
            return response.data;
        } else {
            console.warn('Unexpected users response format:', response.data);
            return [];
        }
    } catch (error) {
        console.error('fetchUsers error:', error);
        return [];
    }
};
