import {useEffect, useRef, useState} from "react";
import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "./ui/card";
import {Button} from "./ui/button";
import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle,} from "./ui/dialog";
import CreateCard from "./CreateCard";
import CreateWorkspaceModal from "./CreateWorkspaceModal";
import AddWorkspaceMemberModal from "./AddWorkspaceMemberModal";
import MembersPreview, {MemberItem} from "./MembersPreview";
import {deleteWorkspace, fetchUserWorkspaces, fetchWorkspaceMembers} from "../api/workSpaceService";
import {WorkspaceResponse} from "../types";
import WorkspaceSkeleton from "./WorkSpaceSkeleton.tsx";

const WORKSPACE_ROLE_NAMES: Record<number, string> = {
    0: 'Creator',
    1: 'Admin',
    2: 'Member',
    3: 'Viewer',
};

interface WorkspaceCardProps {
    selectedWorkspaceId?: string | null;
    onSelect?: (workspaceId: string) => void;
}

export default function WorkspaceCard({selectedWorkspaceId, onSelect}: WorkspaceCardProps) {
    const [workspaces, setWorkspaces] = useState<WorkspaceResponse[]>([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [workspaceToDelete, setWorkspaceToDelete] = useState<WorkspaceResponse | null>(null);

    const [membersMap, setMembersMap] = useState<Record<string, MemberItem[]>>({});
    const [membersLoading, setMembersLoading] = useState<Record<string, boolean>>({});

    const [addMemberWorkspace, setAddMemberWorkspace] = useState<WorkspaceResponse | null>(null);
    const [existingMemberIds, setExistingMemberIds] = useState<string[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    const loadMembers = async (workspaceId: string) => {
        setMembersLoading(prev => ({...prev, [workspaceId]: true}));
        try {
            const response = await fetchWorkspaceMembers(workspaceId);
            const members: MemberItem[] = response.members.map(m => ({
                id: m.id,
                userName: m.userName,
                email: m.email,
                roleName: WORKSPACE_ROLE_NAMES[m.role as number] ?? String(m.role),
            }));
            setMembersMap(prev => ({...prev, [workspaceId]: members}));
        } catch {
            setMembersMap(prev => ({...prev, [workspaceId]: []}));
        } finally {
            setMembersLoading(prev => ({...prev, [workspaceId]: false}));
        }
    };

    useEffect(() => {
        const loadWorkspaces = async () => {
            const data = await fetchUserWorkspaces();
            setIsLoading(true);
            setWorkspaces(data);
            data.forEach(ws => loadMembers(ws.id));
            setIsLoading(false);
        };
        loadWorkspaces();
    }, []);

    const membersMapRef = useRef(membersMap);

    useEffect(() => {
        membersMapRef.current = membersMap;
    }, [membersMap]);

    useEffect(() => {
        const interval = setInterval(() => {
            fetchUserWorkspaces().then(data => {
                setWorkspaces(data);
                data.forEach(ws => {
                    loadMembers(ws.id);

                });
            });
        }, 10000);
        return () => clearInterval(interval);
    }, []);


    const handleWorkspaceCreated = (newWorkspace: WorkspaceResponse) => {
        setWorkspaces(prev => [...prev, newWorkspace]);
        loadMembers(newWorkspace.id);
    };

    const handleWorkspaceDelete = async () => {
        if (!workspaceToDelete) return;
        try {
            await deleteWorkspace(workspaceToDelete.id);
            setWorkspaces(prev => prev.filter(w => w.id !== workspaceToDelete.id));
            setWorkspaceToDelete(null);
        } catch (error) {
            console.error("Ошибка удаления рабочей области:", error);
            alert("Не удалось удалить рабочую область");
        }
    };

    const handleOpenAddMember = async (e: React.MouseEvent, workspace: WorkspaceResponse) => {
        e.stopPropagation();
        try {
            const response = await fetchWorkspaceMembers(workspace.id);
            setExistingMemberIds(response.members.map(m => m.id));
        } catch {
            setExistingMemberIds([]);
        }
        setAddMemberWorkspace(workspace);
    };

    const handleMemberAdded = (workspaceId: string) => {
        loadMembers(workspaceId);
        setAddMemberWorkspace(null);
    };

    const CanDeleteWorkSpace = (workspace: WorkspaceResponse): boolean => {
        return workspace.role == 0;
    }

    const CanAddUsers = (workspace: WorkspaceResponse): boolean => {
        return workspace.role == 0 || workspace.role == 1;
    }

    return (
        <>
            {isLoading
                ? [...Array(3)].map((_, i) => <WorkspaceSkeleton key={i}/>)
                : workspaces.map(workspace => {
                    const isSelected = selectedWorkspaceId === workspace.id;
                    const members = membersMap[workspace.id] ?? [];
                    const isLoadingMembers = membersLoading[workspace.id] && !membersMap[workspace.id];

                    return (
                        <Card
                            key={workspace.id}
                            className={`relative mx-auto w-full max-w-sm cursor-pointer transition-colors ${
                                isSelected ? "border-blue-500" : "border-gray-300 hover:border-blue-500"
                            }`}
                            onClick={() => onSelect?.(workspace.id)}
                        >
                            <div
                                className="absolute right-3 top-3 z-10 flex items-center gap-1"
                                onClick={e => e.stopPropagation()}
                            >
                                {CanAddUsers(workspace) && (
                                    <button
                                        type="button"
                                        className="text-gray-400 hover:text-indigo-600 transition-colors p-1 rounded hover:bg-indigo-50"
                                        title="Добавить участника"
                                        onClick={e => handleOpenAddMember(e, workspace)}
                                    >
                                        <svg width="15" height="15" viewBox="0 0 20 20" fill="currentColor">
                                            <path d="M8 9a3 3 0 100-6 3 3 0 000 6zM8 11a6 6 0 00-6 6h12a6 6 0 00-6-6z"/>
                                            <path d="M16 7v2m0 2v-2m0 0h-2m2 0h2" stroke="currentColor"
                                                  strokeWidth="1.5"
                                                  strokeLinecap="round" fill="none"/>
                                        </svg>
                                    </button>
                                )}
                                {CanDeleteWorkSpace(workspace) &&
                                    (
                                        <button
                                            type="button"
                                            className="text-gray-400 hover:text-red-600 transition-colors p-1 rounded hover:bg-red-50"
                                            title="Удалить рабочую область"
                                            onClick={e => {
                                                e.stopPropagation();
                                                setWorkspaceToDelete(workspace);
                                            }}
                                        >
                                            <svg width="15" height="15" viewBox="0 0 20 20" fill="currentColor">
                                                <path fillRule="evenodd"
                                                      d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
                                                      clipRule="evenodd"/>
                                            </svg>
                                        </button>
                                    )
                                }
                            </div>

                            <CardHeader>
                                <CardTitle className="text-xl pr-16">{workspace.name}</CardTitle>
                                {workspace.createdDate && (
                                    <CardDescription>
                                        Создано: {new Date(workspace.createdDate).toLocaleDateString()}
                                    </CardDescription>
                                )}
                            </CardHeader>

                            <CardContent>
                                <MembersPreview
                                    members={members}
                                    entityName={workspace.name}
                                    isLoading={isLoadingMembers}
                                />
                            </CardContent>
                        </Card>
                    );
                })}

            <CreateCard title="Создать рабочую область" onClick={() => setIsModalOpen(true)}/>

            <CreateWorkspaceModal
                isOpen={isModalOpen}
                onOpenChange={setIsModalOpen}
                onSuccess={handleWorkspaceCreated}
            />

            {addMemberWorkspace && (
                <AddWorkspaceMemberModal
                    isOpen={!!addMemberWorkspace}
                    onOpenChange={open => {
                        if (!open) setAddMemberWorkspace(null);
                    }}
                    workspaceId={addMemberWorkspace.id}
                    existingMemberIds={existingMemberIds}
                    onSuccess={() => handleMemberAdded(addMemberWorkspace.id)}
                />
            )}

            <Dialog open={!!workspaceToDelete} onOpenChange={open => {
                if (!open) setWorkspaceToDelete(null);
            }}>
                <DialogContent className="sm:max-w-md bg-white text-black border-gray-700">
                    <DialogHeader>
                        <DialogTitle>Подтвердите удаление</DialogTitle>
                        <DialogDescription>
                            {workspaceToDelete ? `Вы действительно хотите удалить рабочую область "${workspaceToDelete.name}"?` : ""}
                        </DialogDescription>
                    </DialogHeader>
                    <DialogFooter>
                        <Button variant="outline" onClick={() => setWorkspaceToDelete(null)}>Отмена</Button>
                        <Button variant="destructive" onClick={handleWorkspaceDelete}>Удалить</Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </>
    );
}
