// Auto-generated DTO types for TaskService API
export interface TagCreateRequest {
  name: string;
  projectId: string;
}

export interface TagUpdateRequest {
  id: string;
  name: string;
  color?: string;
}

export interface TagResponse {
  id: string;
  name: string;
  projectId: string;
}
