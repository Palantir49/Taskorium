import {useTasks} from "../context/TaskContext";
import {Gantt, Task as GanttTask, ViewMode} from 'gantt-task-react';
import 'gantt-task-react/dist/index.css';
import {IssueStatusResponse, Task} from "../types";
import {getStableColorById} from "../utils/statusColor.ts";
import {useEffect, useState} from "react";
import {fetchIssueStatusesByProjectId} from "../api/projectService.ts";
import './GantTimelineView.css';


const toGanttTasks = (tasks: Task[], statuses: IssueStatusResponse[]): GanttTask[] =>
    tasks
        .filter(t => t.dueDate)
        .map(t => {
            const status = statuses.find(s => s.id === t.taskStatusId);
            const color = getStableColorById(status?.position ?? 0);

            return {
                id: t.id,
                name: t.name,
                start: new Date(t.createdDate),
                end: new Date(t.dueDate!),
                progress: 0,
                type: 'task',
                styles: {
                    backgroundColor: color,
                    backgroundSelectedColor: color,
                    progressColor: color,
                    progressSelectedColor: color,
                }
            };
        });


const VIEW_MODES = [
    {label: 'Неделя', value: ViewMode.Week},
    {label: 'Месяц', value: ViewMode.Month},
];

const TASK_NAME_WIDTH = 200;
const DATE_COL_WIDTH = 90;
const TOTAL_WIDTH = TASK_NAME_WIDTH + DATE_COL_WIDTH * 2;

const CustomHeader = () => (
    <div
        className="gantt-header"
        style={{width: TOTAL_WIDTH, minWidth: TOTAL_WIDTH}}
    >
        <span style={{width: TASK_NAME_WIDTH}}>Задача</span>
        <span style={{width: DATE_COL_WIDTH}}>Начало</span>
        <span style={{width: DATE_COL_WIDTH}}>Конец</span>
    </div>
);

const formatDate = (date: Date): string =>
    date.toLocaleDateString('ru', {day: '2-digit', month: '2-digit', year: '2-digit'});

const CustomTaskList = ({tasks}: { tasks: GanttTask[] }) => (
    <div className="gantt-task-list">
        {tasks.map(task => (
            <div
                key={task.id}
                className="gantt-task-row"
                style={{width: TOTAL_WIDTH, minWidth: TOTAL_WIDTH}}
            >
                <span className="gantt-task-name" style={{width: TASK_NAME_WIDTH}}>
                    {task.name}
                </span>
                <span className="gantt-task-date" style={{width: DATE_COL_WIDTH}}>
                    {formatDate(task.start)}
                </span>
                <span className="gantt-task-date" style={{width: DATE_COL_WIDTH}}>
                    {formatDate(task.end)}
                </span>
            </div>
        ))}
    </div>
);

export default function GantTimelineView({projectId}: { projectId: string }) {
    const {tasks, loading} = useTasks();
    const [statuses, setStatuses] = useState<IssueStatusResponse[]>([]);

    useEffect(() => {
        fetchIssueStatusesByProjectId(projectId)
            .then(data => setStatuses([...(data ?? [])].sort((a, b) => a.position - b.position)));
    }, [projectId]);
    const gantTasks = toGanttTasks(tasks, statuses);
    const [viewMode, setViewMode] = useState(ViewMode.Week);
    if (loading) {
        return <div className="timeline-container">Загрузка...</div>;
    }

    if (gantTasks.length === 0) {
        return (
            <div className="timeline-container">
                <p className="text-gray-500 text-sm">Нет задач с указанным дедлайном</p>
            </div>
        );
    }
    return (
        <div className="timeline-container">
            <div className="timeline-toolbar">
                {VIEW_MODES.map(mode => (
                    <button
                        key={mode.value}
                        className={`timeline-mode-btn ${viewMode === mode.value ? 'active' : ''}`}
                        onClick={() => setViewMode(mode.value)}
                    >
                        {mode.label}
                    </button>
                ))}
            </div>
            <div className="timeline-gantt-wrapper">
                <Gantt
                    tasks={gantTasks}
                    viewMode={viewMode}
                    TaskListHeader={CustomHeader}
                    TaskListTable={CustomTaskList}
                    columnWidth={viewMode === ViewMode.Week ? 150 : 200}
                    locale={'ru'}
                />
            </div>
        </div>
    );
}