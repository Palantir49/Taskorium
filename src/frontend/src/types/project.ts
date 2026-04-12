// Auto-generated DTO types for TaskService API
import { RoleDto } from './common';

export interface ProjectMemberDto {
  projectId: string;
  userId: string;
  roleDto: RoleDto;
}

export interface CreateProjectRequest {
  name: string;
  description: string;
  abbreviation: string;
  workspaceId: string;
}

export interface UpdateProjectRequest {
  name: string;
  description: string;
}

export interface ProjectResponse {
  id: string;
  name: string;
  description?: string;
  abbreviation: string;
  workspaceId: string;
  createdDate: string;
}
