import { IssueTypeResponse, IssueStatusTypeResponse, IssuePriorityResponse } from '../types/issue';
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
