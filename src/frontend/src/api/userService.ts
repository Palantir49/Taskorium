import {UserResponse} from "../types";
import {api} from "./axios.ts";

export const fetchAllUsers = async (skip = 0, take = 200): Promise<UserResponse[]> => {
    try {
        const response = await api.get(`/User/users`, {params: {skip, take}});
        return response.data?.users ?? [];
    } catch (error) {
        console.error('fetchAllUsers error:', error);
        throw error;
    }
};