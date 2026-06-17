
// Экспорт DTO
export * from './common';
export * from './attachment';
export * from './issue';
export * from './issueStatus';
export * from './project';
export * from './tag';
export * from './user';
export * from './workspace';

// Типы для приложения 
import { IssueResponse, IssueCreateRequest, UpdateIssueRequest, IssuePriorityResponse, IssueTypeResponse } from './issue';
import { IssueStatusResponse, IssueStatusTypeResponse } from './issueStatus';
import { UserResponse } from './user';
import { ProjectResponse } from './project';

// Типы для приложения
export type Task = IssueResponse;
export type CreateTaskData = IssueCreateRequest;
export type UpdateTaskData = UpdateIssueRequest;
export type TaskStatus = IssueStatusTypeResponse['name'];
export type TaskPriority = IssuePriorityResponse['name'];
export type TaskType = IssueTypeResponse['name'];
export type User = UserResponse;
export type Project = ProjectResponse;
export type TaskStatusType = IssueStatusResponse;

// Подзадачи (в бэке отсутствуют)
export interface Subtask {
    id: number;
    title: string;
    completed: boolean;
}

// Типы для фильтрации
export interface TaskFilters {
    assignedTo: string;
    type: string;
    priority: string;
    createdAt: string;
    deadline: string;
}

// Состояние приложения
export interface TaskState {
    tasks: Task[];
    loading: boolean;
    error: string | null;
    selectedTask: Task | null;
    filters: TaskFilters;
}

// Типы для действий reducer
export type ActionType =
    | 'SET_LOADING'
    | 'SET_TASKS'
    | 'SET_ERROR'
    | 'SET_SELECTED_TASK'
    | 'UPDATE_TASK'
    | 'ADD_TASK'
    | 'REMOVE_TASK'
    | 'SET_FILTER'
    | 'RESET_FILTERS';

export interface Action {
    type: ActionType;
    payload?: any;
}

// Типы для компонентов
export interface TaskCardProps {
    task: Task;
    onClick?: (task: Task) => void;
}

export interface ColumnProps {
    status: TaskStatus;
    tasks: Task[];
    onTaskClick?: (task: Task) => void;
    isSidebarOpen?: boolean;
    onCreateTask?: (status: TaskStatus) => void;
}

export interface FilterBarProps {

}

export interface HeaderProps {
    activeTab: string;
    onTabChange: (tab: string) => void,
    authInfo: AuthInfo;
}

export interface KanbanBoardProps {
    
}

export interface TaskDetailSidebarProps {
    
}

export interface TaskCreateFormProps {
    isOpen: boolean;
    onClose: () => void;
    projectId: string;
    initialStatus?: TaskStatus;
    mode?: 'create' | 'edit';
    task?: Task | null;
    onSaved?: () => void;
    onAttachmentsChanged?: () => void | Promise<void>;
}

export interface AuthInfo {
    isAuthenticated: boolean;
    userFullName: string,
    onLogin: () => void,
    onLogout: () => void
}

export interface AuthProviderProps {
    children: React.ReactNode;
    activeTab?: string;
    onTabChange?: (tab: string) => void;
    showHeader?: boolean;
}