import {IssueStatusCreateRequest, IssueStatusResponse, IssueStatusUpdateRequest} from '../types';
import {api} from "./axios.ts";


/**
 * Получить статус задачи по ID
 */
export const fetchIssueStatusById = async (id: string): Promise<IssueStatusResponse> => {
    try {
        const response = await api.get(`/IssueStatuses/${id}`);
        console.log('fetchIssueStatusById:', response.data);
        return response.data;
    } catch (error) {
        console.error('fetchIssueStatusById error:', error);
        throw error;
    }
};

/**
 * Создать новый статус задачи
 */
export const createIssueStatus = async (statusData: IssueStatusCreateRequest): Promise<IssueStatusResponse> => {
    console.log('createIssueStatus called:', statusData);
    try {
        const response = await api.post('/IssueStatuses', statusData);
        console.log('createIssueStatus:', response.data);
        return response.data;
    } catch (error) {
        console.error('createIssueStatus error:', error);
        throw error;
    }
};

/**
 * Обновить статус задачи
 */
export const updateIssueStatus = async (id: string, updates: IssueStatusUpdateRequest): Promise<IssueStatusResponse> => {
    console.log('updateIssueStatus called:', id, 'updates:', updates);
    try {
        const response = await api.put(`/IssueStatuses/${id}`, updates);
        console.log('updateIssueStatus:', response.data);
        return response.data;
    } catch (error) {
        console.error('updateIssueStatus error:', error);
        throw error;
    }
};

/**
 * Удалить статус задачи
 */
export const deleteIssueStatus = async (id: string): Promise<void> => {
    console.log('deleteIssueStatus:', id);
    try {
        const response = await api.delete(`/IssueStatuses/${id}`);
        console.log('deleteIssueStatus:', response.status);
    } catch (error) {
        console.error('deleteIssueStatus error:', error);
        throw error;
    }
};
