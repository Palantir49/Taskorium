import { TagResponse, TagCreateRequest, TagUpdateRequest } from '../types/tag';
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

api.interceptors.response.use(
  (response) => {
    console.log('API Response:', response.status, response.config.method?.toUpperCase(), response.config.url, response.data);
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.status, error.config?.method?.toUpperCase(), error.config?.url, error.response?.data);
    return Promise.reject(error);
  }
);

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
