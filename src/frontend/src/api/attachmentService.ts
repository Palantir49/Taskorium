import { api } from './axios.ts';

export const downloadAttachment = async (id: string): Promise<Blob> => {
    const response = await api.get(`/Attachments/${id}`, {
        responseType: 'blob',
    });

    return response.data;
};

export const deleteAttachment = async (id: string): Promise<void> => {
    await api.delete(`/Attachments/${id}`);
};