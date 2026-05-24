// Auto-generated DTO types for TaskService API
import {RoleDto} from './common';

export interface WorkspaceMemberDto {
    workspaceId: string;
    userId: string;
    roleDto: RoleDto;
}

export interface CreateWorkspaceRequest {
    name: string;
}

export interface UpdateWorkspaceRequest {
    name: string;
}

export interface AddUserToWorkspaceRequest {
    userId: string;
    role: RoleDto;
}

export interface WorkspaceResponse {
    id: string;
    name: string;
    createdDate: string;
    ownerId?: string;
}
