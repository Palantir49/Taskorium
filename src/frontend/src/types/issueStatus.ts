// Auto-generated DTO types for TaskService API
export interface IssueStatusTypeResponse {
  number: number;
  name: string;
  displayName: string;
}

export interface IssueStatusResponse {
  id: string;
  name: string;
  projectId: string;
  type: string;
  position: number;
}

export interface IssueStatusCreateRequest {
  name: string;
  projectId: string;
  numberType: number;
  position: number;
  color?: string;
}

export interface IssueStatusUpdateRequest {
  name: string;
  type: string;
  position: number;
  color?: string;
}
