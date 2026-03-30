// Auto-generated DTO types for TaskService API
export interface IssuePriorityResponse {
  number: number;
  name: string;
  displayName: string;
}

export interface IssueTypeResponse {
  number: number;
  name: string;
  displayName: string;
  code: string;
}

export interface AddTagToIssueRequest {
  tagId: string;
}

export interface GetIssuesRequest {

}

export interface IssueCreateRequest {
  name: string;
  projectId: string;
  numberIssueType: number;
  numberIssuePriority: number;
  description?: string;
  dueDate?: string;
}

export interface UpdateIssueRequest {
  name: string;
  issueStatusId: string;
  numberIssueType: number;
  numberIssuePriority: number;
  description?: string;
  dueDate?: string;
}

export interface IssueResponse {
  id: string;
  name: string;
  description?: string;
  projectId: string;
  taskStatusId: string;
  issueType: IssueTypeResponse;
  issuePriority: IssuePriorityResponse;
  createdDate: string;
  updatedDate?: string;
  dueDate?: string;
  resolvedDate?: string;
}

export interface IssuesResponse {
  issues: Array<IssueResponse>;
}
