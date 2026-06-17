import { api } from './axios.ts';
import {AttachmentResponse} from '../types';

export const downloadAttachment = async (id: string): Promise<Blob> => {
    const response = await api.get(`/Attachments/${id}`, {
        responseType: 'blob',
    });

    return response.data;
};

export const deleteAttachment = async (id: string): Promise<void> => {
    await api.delete(`/Attachments/${id}`);
};

export const addAttachmentsToIssue = async (issueId: string, files: File[]): Promise<AttachmentResponse[]> => {
    const formData = new FormData();
    files.forEach((file) => formData.append('Attachments', file));

    const response = await api.post(`/Issues/${issueId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });

    return response.data;
};