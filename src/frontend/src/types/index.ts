// Основные типы данных для приложения Taskorium

export interface User {
    id: number;
    name: string;
    initials: string;
}

export interface Subtask {
    id: number;
    title: string;
    completed: boolean;
}

export type TaskStatus = 'backlog' | 'in-progress' | 'testing' | 'pause' | 'done';

export type TaskPriority = 'low' | 'medium' | 'high' | 'critical';

export type TaskType = 'bug' | 'feature' | 'improvement';

export interface Task {
    id: number;
    title: string;
    description: string;
    status: TaskStatus;
    priority: TaskPriority;
    type: TaskType;
    assignedTo: User;
    createdAt: Date;
    deadline: Date | null;
    subtasks: Subtask[];
}

export interface TaskFilters {
    assignedTo: string;
    type: string;
    priority: string;
    createdAt: string;
    deadline: string;
}

export interface TaskState {
    tasks: Task[];
    loading: boolean;
    error: string | null;
    selectedTask: Task | null;
    filters: TaskFilters;
}

// Типы для действий reducer'а
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

// Типы для API
export interface CreateTaskData {
    title: string;
    description: string;
    status?: TaskStatus;
    priority?: TaskPriority;
    type?: TaskType;
    assignedTo?: User;
    deadline?: Date | null;
    subtasks?: Omit<Subtask, 'id'>[];
}

export interface UpdateTaskData {
    title?: string;
    description?: string;
    status?: TaskStatus;
    priority?: TaskPriority;
    type?: TaskType;
    assignedTo?: User;
    deadline?: Date | null;
    subtasks?: Subtask[];
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
    // Props для FilterBar компонента
}

export interface HeaderProps {
    activeTab: string;
    onTabChange: (tab: string) => void,
    authInfo: AuthInfo;
}

export interface KanbanBoardProps {
    // Props для KanbanBoard компонента (пока пустой)
}

export interface TaskDetailSidebarProps {
    // Props для TaskDetailSidebar компонента
}

export interface TaskCreateFormProps {
    isOpen: boolean;
    onClose: () => void;
    initialStatus?: TaskStatus;
}

export interface AuthInfo {
    isAuthenticated: boolean;
    userFullName: string,
    onLogin: () => void,
    onLogout: () => void
}
