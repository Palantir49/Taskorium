// Auto-generated DTO types for TaskService API
export interface CreateUserRequest {
    name: string;
    keycloakId: string;
    email: string;
    username: string;
}

export interface GetUserRequest {
    id: string;
}

export interface UserResponse {
    id: string;
    keycloakId: string;
    email?: string;
    userName?: string;
    createdAt?: string;
}
