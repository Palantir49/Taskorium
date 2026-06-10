import {useEffect, useState} from "react";
import {Card, CardContent, CardHeader, CardTitle} from "./ui/card";
import {Button} from "./ui/button";
import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle,} from "./ui/dialog";
import CreateCard from "./CreateCard";
import CreateProjectModal from "./CreateProjectModal";
import AddProjectMemberModal from "./AddProjectMemberModal";
import MembersPreview, {MemberItem} from "./MembersPreview";
import {deleteProject, fetchProjectMembers, fetchProjectsByWorkspaceId} from "../api/projectService";
import {Project, ProjectResponse} from "../types";

const PROJECT_ROLE_NAMES: Record<number, string> = {
    0: 'Creator',
    1: 'Admin',
    2: 'Member',
    3: 'Viewer',
};

interface ProjectCardProps {
    workspaceId?: string;
    onSelect?: (projectId: string) => void;
}

export default function ProjectCard({workspaceId, onSelect}: ProjectCardProps) {
    const [projects, setProjects] = useState<ProjectResponse[]>([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [projectToDelete, setProjectToDelete] = useState<ProjectResponse | null>(null);

    const [membersMap, setMembersMap] = useState<Record<string, MemberItem[]>>({});
    const [membersLoading, setMembersLoading] = useState<Record<string, boolean>>({});

    const [addMemberProject, setAddMemberProject] = useState<ProjectResponse | null>(null);
    const [existingMemberIds, setExistingMemberIds] = useState<string[]>([]);

    const loadMembers = async (projectId: string) => {
        setMembersLoading(prev => ({...prev, [projectId]: true}));
        try {
            const response = await fetchProjectMembers(projectId);
            const members: MemberItem[] = response.members.map(m => ({
                id: m.id,
                userName: m.userName,
                email: m.email,
                roleName: PROJECT_ROLE_NAMES[m.role as number] ?? String(m.role),
            }));
            setMembersMap(prev => ({...prev, [projectId]: members}));
        } catch {
            setMembersMap(prev => ({...prev, [projectId]: []}));
        } finally {
            setMembersLoading(prev => ({...prev, [projectId]: false}));
        }
    };

    useEffect(() => {
        if (!workspaceId) {
            setProjects([]);
            setMembersMap({});
            return;
        }
        const loadProjects = async () => {
            const data = await fetchProjectsByWorkspaceId(workspaceId);
            setProjects(data);
            data.forEach(p => loadMembers(p.id));
        };
        loadProjects();
    }, [workspaceId]);

    const handleProjectCreated = (newProject: ProjectResponse) => {
        setProjects(prev => [...prev, newProject]);
        loadMembers(newProject.id);
    };

    const handleProjectDelete = async () => {
        if (!projectToDelete) return;
        try {
            await deleteProject(projectToDelete.id);
            setProjects(prev => prev.filter(p => p.id !== projectToDelete.id));
            setProjectToDelete(null);
        } catch (error) {
            console.error("Ошибка удаления проекта:", error);
            alert("Не удалось удалить проект");
        }
    };

    const handleOpenAddMember = async (e: React.MouseEvent, project: ProjectResponse) => {
        e.stopPropagation();
        try {
            const response = await fetchProjectMembers(project.id);
            setExistingMemberIds(response.members.map(m => m.id));
        } catch {
            setExistingMemberIds([]);
        }
        setAddMemberProject(project);
    };

    const handleMemberAdded = (projectId: string) => {
        loadMembers(projectId);
        setAddMemberProject(null);
    };

    const canAddMembersToProject = (project: Project): boolean => {
        return project.role == 0 || project.role == 1;
    }

    const canDeleteProject = (project: Project): boolean => {
        return project.role == 0;
    }

    return (
        <>
            {projects.map(project => {
                const members = membersMap[project.id] ?? [];
                const isLoadingMembers = membersLoading[project.id] ?? true;

                return (
                    <Card
                        key={project.id}
                        className="relative border-gray-300 cursor-pointer hover:border-blue-500 transition-colors"
                        onClick={() => onSelect?.(project.id)}
                    >
                        <div
                            className="absolute right-3 top-3 z-10 flex items-center gap-1"
                            onClick={e => e.stopPropagation()}
                        >
                            {canAddMembersToProject(project) && (
                                <button
                                    type="button"
                                    className="text-gray-400 hover:text-indigo-600 transition-colors p-1 rounded hover:bg-indigo-50"
                                    title="Добавить участника"
                                    onClick={e => handleOpenAddMember(e, project)}
                                >
                                    <svg width="15" height="15" viewBox="0 0 20 20" fill="currentColor">
                                        <path d="M8 9a3 3 0 100-6 3 3 0 000 6zM8 11a6 6 0 00-6 6h12a6 6 0 00-6-6z"/>
                                        <path d="M16 7v2m0 2v-2m0 0h-2m2 0h2" stroke="currentColor" strokeWidth="1.5"
                                              strokeLinecap="round" fill="none"/>
                                    </svg>
                                </button>
                            )
                            }
                            {canDeleteProject(project) && (
                                <button
                                    type="button"
                                    className="text-gray-400 hover:text-red-600 transition-colors p-1 rounded hover:bg-red-50"
                                    title="Удалить проект"
                                    onClick={e => {
                                        e.stopPropagation();
                                        setProjectToDelete(project);
                                    }}
                                >
                                    <svg width="15" height="15" viewBox="0 0 20 20" fill="currentColor">
                                        <path fillRule="evenodd"
                                              d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
                                              clipRule="evenodd"/>
                                    </svg>
                                </button>
                            )}
                        </div>

                        <CardHeader className="pb-3">
                            <CardTitle className="text-lg pr-16">{project.name}</CardTitle>
                        </CardHeader>

                        <CardContent className="space-y-2">
                            {project.description && (
                                <div className="text-sm text-gray-600">{project.description}</div>
                            )}
                            <div className="text-sm text-gray-600">
                                Аббревиатура: <span className="font-semibold">{project.abbreviation}</span>
                            </div>
                            <MembersPreview
                                members={members}
                                entityName={project.name}
                                isLoading={isLoadingMembers}
                            />
                        </CardContent>
                    </Card>
                );
            })}

            {workspaceId && (
                <CreateCard title="Создать проект" onClick={() => setIsModalOpen(true)}/>
            )}

            {workspaceId && (
                <CreateProjectModal
                    isOpen={isModalOpen}
                    onOpenChange={setIsModalOpen}
                    workspaceId={workspaceId}
                    onSuccess={handleProjectCreated}
                />
            )}

            {addMemberProject && workspaceId && (
                <AddProjectMemberModal
                    isOpen={!!addMemberProject}
                    onOpenChange={open => {
                        if (!open) setAddMemberProject(null);
                    }}
                    projectId={addMemberProject.id}
                    workspaceId={workspaceId}
                    existingMemberIds={existingMemberIds}
                    onSuccess={() => handleMemberAdded(addMemberProject.id)}
                />
            )}

            <Dialog open={!!projectToDelete} onOpenChange={open => {
                if (!open) setProjectToDelete(null);
            }}>
                <DialogContent className="sm:max-w-md bg-white text-black border-gray-700">
                    <DialogHeader>
                        <DialogTitle>Подтвердите удаление</DialogTitle>
                        <DialogDescription>
                            {projectToDelete ? `Вы действительно хотите удалить проект "${projectToDelete.name}"?` : ""}
                        </DialogDescription>
                    </DialogHeader>
                    <DialogFooter>
                        <Button variant="outline" onClick={() => setProjectToDelete(null)}>Отмена</Button>
                        <Button variant="destructive" onClick={handleProjectDelete}>Удалить</Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>

            {!workspaceId && (
                <div className="text-center text-gray-500 py-8">
                    Выберите рабочую область для создания проектов
                </div>
            )}
        </>
    );
}
