import React, { createContext, useContext, useReducer, useEffect, ReactNode } from 'react';
import { fetchTasks, updateTask, createTask, deleteTask, fetchTasksTestController } from '../api/taskService';
import { Task, TaskState, Action, ActionType, UpdateTaskData, CreateTaskData } from '../types';

const TaskContext = createContext<TaskState & {
  loadTasks: () => Promise<void>;
  updateTask: (id: number, updates: UpdateTaskData) => Promise<Task>;
  createTask: (taskData: CreateTaskData) => Promise<Task>;
  deleteTask: (id: number) => Promise<void>;
  setSelectedTask: (task: Task | null) => void;
  setFilter: (key: keyof TaskState['filters'], value: string) => void;
  resetFilters: () => void;
} | undefined>(undefined);

// Начальное состояние
const initialState: TaskState = {
  tasks: [],
  loading: false,
  error: null,
  selectedTask: null,
  filters: {
    assignedTo: '',
    type: '',
    priority: '',
    createdAt: '',
    deadline: ''
  }
};

// Типы действий
const ActionTypes: Record<string, ActionType> = {
  SET_LOADING: 'SET_LOADING',
  SET_TASKS: 'SET_TASKS',
  SET_ERROR: 'SET_ERROR',
  SET_SELECTED_TASK: 'SET_SELECTED_TASK',
  UPDATE_TASK: 'UPDATE_TASK',
  ADD_TASK: 'ADD_TASK',
  REMOVE_TASK: 'REMOVE_TASK',
  SET_FILTER: 'SET_FILTER',
  RESET_FILTERS: 'RESET_FILTERS'
};

// Редьюсер для управления состоянием
function taskReducer(state: TaskState, action: Action): TaskState {
  switch (action.type) {
    case ActionTypes.SET_LOADING:
      return { ...state, loading: action.payload as boolean };

    case ActionTypes.SET_TASKS:
      return { ...state, tasks: action.payload as Task[], loading: false, error: null };

    case ActionTypes.SET_ERROR:
      return { ...state, error: action.payload as string, loading: false };

    case ActionTypes.SET_SELECTED_TASK:
      return { ...state, selectedTask: action.payload as Task | null };

    case ActionTypes.UPDATE_TASK:
      return {
        ...state,
        tasks: state.tasks.map(task =>
          task.id === (action.payload as Task).id ? action.payload as Task : task
        ),
        selectedTask: state.selectedTask?.id === (action.payload as Task).id
          ? action.payload as Task
          : state.selectedTask
      };

    case ActionTypes.ADD_TASK:
      return {
        ...state,
        tasks: [...state.tasks, action.payload as Task]
      };

    case ActionTypes.REMOVE_TASK:
      return {
        ...state,
        tasks: state.tasks.filter(task => task.id !== action.payload),
        selectedTask: state.selectedTask?.id === action.payload ? null : state.selectedTask
      };

    case ActionTypes.SET_FILTER:
      return {
        ...state,
        filters: {
          ...state.filters,
          [(action.payload as { key: string; value: string }).key]: (action.payload as { key: string; value: string }).value
        }
      };

    case ActionTypes.RESET_FILTERS:
      return {
        ...state,
        filters: initialState.filters
      };

    default:
      return state;
  }
}

interface TaskProviderProps {
  children: ReactNode;
}

// Провайдер контекста
export function TaskProvider({ children }: TaskProviderProps) {
  const [state, dispatch] = useReducer(taskReducer, initialState);

  // Загрузка задач при монтировании
  useEffect(() => {
    loadTasks();
    // Вызов тестового метода для проверки
    fetchTasksTestController();
  }, []);

  const loadTasks = async (): Promise<void> => {
    dispatch({ type: ActionTypes.SET_LOADING, payload: true });
    try {
      const tasks = await fetchTasks();
      dispatch({ type: ActionTypes.SET_TASKS, payload: tasks });
    } catch (error) {
      dispatch({ type: ActionTypes.SET_ERROR, payload: (error as Error).message });
    }
  };

  const handleUpdateTask = async (id: number, updates: UpdateTaskData): Promise<Task> => {
    try {
      const updatedTask = await updateTask(id, updates);
      dispatch({ type: ActionTypes.UPDATE_TASK, payload: updatedTask });
      return updatedTask;
    } catch (error) {
      dispatch({ type: ActionTypes.SET_ERROR, payload: (error as Error).message });
      throw error;
    }
  };

  const handleCreateTask = async (taskData: CreateTaskData): Promise<Task> => {
    try {
      const newTask = await createTask(taskData);
      dispatch({ type: ActionTypes.ADD_TASK, payload: newTask });
      return newTask;
    } catch (error) {
      dispatch({ type: ActionTypes.SET_ERROR, payload: (error as Error).message });
      throw error;
    }
  };

  const handleDeleteTask = async (id: number): Promise<void> => {
    try {
      await deleteTask(id);
      dispatch({ type: ActionTypes.REMOVE_TASK, payload: id });
    } catch (error) {
      dispatch({ type: ActionTypes.SET_ERROR, payload: (error as Error).message });
      throw error;
    }
  };

  const setSelectedTask = (task: Task | null): void => {
    dispatch({ type: ActionTypes.SET_SELECTED_TASK, payload: task });
  };

  const setFilter = (key: keyof TaskState['filters'], value: string): void => {
    dispatch({ type: ActionTypes.SET_FILTER, payload: { key, value } });
  };

  const resetFilters = (): void => {
    dispatch({ type: ActionTypes.RESET_FILTERS });
  };

  const value = {
    ...state,
    loadTasks,
    updateTask: handleUpdateTask,
    createTask: handleCreateTask,
    deleteTask: handleDeleteTask,
    setSelectedTask,
    setFilter,
    resetFilters
  };

  return (
    <TaskContext.Provider value={value}>
      {children}
    </TaskContext.Provider>
  );
}

// Хук для использования контекста
export function useTasks() {
  const context = useContext(TaskContext);
  if (!context) {
    throw new Error('useTasks must be used within a TaskProvider');
  }
  return context;
}
