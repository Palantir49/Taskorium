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
    role: number;
}

export interface WorkspaceResponse {
    id: string;
    name: string;
    createdDate: string;
    ownerId?: string;
    role: number;
}

export interface WorkspaceRolesDto {
    number: number;
    name: string;
    displayName: string;
}

export interface WorkspaceUserDto {
    id: string;
    keycloakId: string;
    role: number;
    joinedAt: string;
    email?: string | null;
    userName?: string | null;
}

export interface WorkspaceMembersResponse {
    workspaceId: string;
    workspaceName: string;
    members: WorkspaceUserDto[];
}
