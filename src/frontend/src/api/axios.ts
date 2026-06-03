// api/axios.ts
import axios from 'axios';

let tokenProvider: () => string | null = () => null;

export const setTokenProvider = (provider: () => string | null) => {
    tokenProvider = provider;
};

export const api = axios.create({
    baseURL: '/api/v1',
    headers: {'Content-Type': 'application/json'},
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
        if (error.response?.status === 401) {
            console.log('401 Unauthorized');
        }
        return Promise.reject(error);
    }
);