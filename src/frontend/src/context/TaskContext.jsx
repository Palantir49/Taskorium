import React, { createContext, useContext, useReducer, useEffect } from 'react';
import { fetchTasks, updateTask, createTask, deleteTask, fetchTasksTestController } from '../api/taskService';

const TaskContext = createContext();

// Начальное состояние
const initialState = {
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
const ActionTypes = {
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
function taskReducer(state, action) {
  switch (action.type) {
    case ActionTypes.SET_LOADING:
      return { ...state, loading: action.payload };
    
    case ActionTypes.SET_TASKS:
      return { ...state, tasks: action.payload, loading: false, error: null };
    
    case ActionTypes.SET_ERROR:
      return { ...state, error: action.payload, loading: false };
    
    case ActionTypes.SET_SELECTED_TASK:
      return { ...state, selectedTask: action.payload };
    
    case ActionTypes.UPDATE_TASK:
      return {
        ...state,
        tasks: state.tasks.map(task =>
          task.id === action.payload.id ? action.payload : task
        ),
        selectedTask: state.selectedTask?.id === action.payload.id
          ? action.payload
          : state.selectedTask
      };
    
    case ActionTypes.ADD_TASK:
      return {
        ...state,
        tasks: [...state.tasks, action.payload]
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
          [action.payload.key]: action.payload.value
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

// Провайдер контекста
export function TaskProvider({ children }) {
  const [state, dispatch] = useReducer(taskReducer, initialState);

  // Загрузка задач при монтировании
  useEffect(() => {
    loadTasks();
    // Вызов тестового метода для проверки
    fetchTasksTestController();
  }, []);

  const loadTasks = async () => {
    dispatch({ type: ActionTypes.SET_LOADING, payload: true });
    try {
      const tasks = await fetchTasks();
      dispatch({ type: ActionTypes.SET_TASKS, payload: tasks });
    } catch (error) {
      dispatch({ type: ActionTypes.SET_ERROR, payload: error.message });
    }
  };

  const handleUpdateTask = async (id, updates) => {
    try {
      const updatedTask = await updateTask(id, updates);
      dispatch({ type: ActionTypes.UPDATE_TASK, payload: updatedTask });
      return updatedTask;
    } catch (error) {
      dispatch({ type: ActionTypes.SET_ERROR, payload: error.message });
      throw error;
    }
  };

  const handleCreateTask = async (taskData) => {
    try {
      const newTask = await createTask(taskData);
      dispatch({ type: ActionTypes.ADD_TASK, payload: newTask });
      return newTask;
    } catch (error) {
      dispatch({ type: ActionTypes.SET_ERROR, payload: error.message });
      throw error;
    }
  };

  const handleDeleteTask = async (id) => {
    try {
      await deleteTask(id);
      dispatch({ type: ActionTypes.REMOVE_TASK, payload: id });
    } catch (error) {
      dispatch({ type: ActionTypes.SET_ERROR, payload: error.message });
      throw error;
    }
  };

  const setSelectedTask = (task) => {
    dispatch({ type: ActionTypes.SET_SELECTED_TASK, payload: task });
  };

  const setFilter = (key, value) => {
    dispatch({ type: ActionTypes.SET_FILTER, payload: { key, value } });
  };

  const resetFilters = () => {
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


