import {TagCreateRequest, TagResponse, TagUpdateRequest} from '../types';
import {api} from "./axios.ts";


/**
 * Получить тег по ID
 */
export const fetchTagById = async (id: string): Promise<TagResponse> => {
    try {
        const response = await api.get(`/Tags/${id}`);
        console.log('fetchTagById:', response.data);
        return response.data;
    } catch (error) {
        console.error('fetchTagById error:', error);
        throw error;
    }
};

/**
 * Создать новый тег
 */
export const createTag = async (tagData: TagCreateRequest): Promise<TagResponse> => {
    console.log('createTag called:', tagData);
    try {
        const response = await api.post('/Tags', tagData);
        console.log('createTag:', response.data);
        return response.data;
    } catch (error) {
        console.error('createTag error:', error);
        throw error;
    }
};

/**
 * Обновить тег
 */
export const updateTag = async (id: string, updates: TagUpdateRequest): Promise<TagResponse> => {
    console.log('updateTag called:', id, 'updates:', updates);
    try {
        const response = await api.put(`/Tags/${id}`, updates);
        console.log('updateTag:', response.data);
        return response.data;
    } catch (error) {
        console.error('updateTag error:', error);
        throw error;
    }
};

/**
 * Удалить тег
 */
export const deleteTag = async (id: string): Promise<void> => {
    console.log('deleteTag:', id);
    try {
        const response = await api.delete(`/Tags/${id}`);
        console.log('deleteTag:', response.status);
    } catch (error) {
        console.error('deleteTag error:', error);
        throw error;
    }
};
