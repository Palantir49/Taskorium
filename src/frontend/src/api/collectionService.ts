import {IssuePriorityResponse, IssueStatusTypeResponse, IssueTypeResponse} from '../types';
import {api} from "./axios";


/**
 * Получить все типы задач
 */
export const fetchIssueTypes = async (): Promise<IssueTypeResponse[]> => {
    try {
        const response = await api.get('/Collections/IssueType');
        console.log('fetchIssueTypes:', response.data);
        return response.data || [];
    } catch (error) {
        console.error('fetchIssueTypes error:', error);
        throw error;
    }
};

/**
 * Получить все типы статусов
 */
export const fetchIssueStatusTypes = async (): Promise<IssueStatusTypeResponse[]> => {
    try {
        const response = await api.get('/Collections/IssueStatusType');
        console.log('fetchIssueStatusTypes:', response.data);
        return response.data || [];
    } catch (error) {
        console.error('fetchIssueStatusTypes error:', error);
        throw error;
    }
};

/**
 * Получить все приоритеты задач
 */
export const fetchIssuePriorities = async (): Promise<IssuePriorityResponse[]> => {
    try {
        const response = await api.get('/Collections/IssuePriority');
        console.log('fetchIssuePriorities:', response.data);
        return response.data || [];
    } catch (error) {
        console.error('fetchIssuePriorities error:', error);
        throw error;
    }
};
