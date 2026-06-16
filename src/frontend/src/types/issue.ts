// Auto-generated DTO types for TaskService API
import type { AttachmentResponse } from './attachment';

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
    dueDate?: string | null;
    assigneeIds?: IssueAssigneesDto[] | null;
}

export interface UpdateIssueRequest {
    name: string;
    issueStatusId: string;
    numberIssueType: number;
    numberIssuePriority: number;
    description?: string;
    dueDate?: string | null;
    assignees?: IssueAssigneesDto[] | null;
}

export interface IssueUpdateStatusRequest {
    newStatusId: string;
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
    updatedDate?: string | null;
    dueDate?: string | null;
    resolvedDate?: string | null;
    assignees?: IssueAssigneesDto[] | null;
    attachments?: AttachmentResponse[] | null;
}

export interface IssuesResponse {
    issues: Array<IssueResponse>;
}

export interface IssueAssigneesDto {
    userId: string;
    role: number;
    userName: string;
}
