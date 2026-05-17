import { useState, useEffect } from "react";
import { Card, CardContent, CardHeader, CardDescription, CardTitle } from "./ui/card";
import { Button } from "./ui/button";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription, DialogFooter } from "./ui/dialog";
import CreateCard from "./CreateCard";
import CreateWorkspaceModal from "./CreateWorkspaceModal";
import { fetchUserWorkspaces, deleteWorkspace } from "../api/workSpaceService";
import { WorkspaceResponse } from "../types/workspace";

interface WorkspaceCardProps {
  selectedWorkspaceId?: string | null;
  onSelect?: (workspaceId: string) => void;
}

export default function WorkspaceCard({ selectedWorkspaceId, onSelect }: WorkspaceCardProps) {
  const [workspaces, setWorkspaces] = useState<WorkspaceResponse[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [workspaceToDelete, setWorkspaceToDelete] = useState<WorkspaceResponse | null>(null);

  useEffect(() => {
    const loadWorkspaces = async () => {
      const data = await fetchUserWorkspaces();
      setWorkspaces(data);
    };

    loadWorkspaces();
  }, []);

  const handleWorkspaceCreated = (newWorkspace: WorkspaceResponse) => {
    setWorkspaces([...workspaces, newWorkspace]);
  };

  const handleWorkspaceDelete = async () => {
    if (!workspaceToDelete) return;
    try {
      await deleteWorkspace(workspaceToDelete.id);
      setWorkspaces((prev) => prev.filter((workspace) => workspace.id !== workspaceToDelete.id));
      setWorkspaceToDelete(null);
    } catch (error) {
      console.error("Ошибка удаления рабочей области:", error);
      alert("Не удалось удалить рабочую область");
    }
  };

  return (
    <>
      {workspaces.map((workspace) => (
        (() => {
          const isSelected = selectedWorkspaceId === workspace.id;
          return (
        <Card 
          key={workspace.id} 
          className={`relative mx-auto w-full max-w-sm cursor-pointer transition-colors ${
            isSelected ? "border-blue-500" : "border-gray-300 hover:border-blue-500"
          }`}
          onClick={() => onSelect?.(workspace.id)}
        >
          <button
            type="button"
            className="absolute right-3 top-3 z-10 text-gray-500 hover:text-red-600"
            title="Удалить рабочую область"
            onClick={(e) => {
              e.stopPropagation();
              setWorkspaceToDelete(workspace);
            }}
          >
            🗑️
          </button>
          <CardHeader>
            <CardTitle className="text-xl">{workspace.name}</CardTitle>
            {workspace.createdDate && (
              <CardDescription>
                Создано: {new Date(workspace.createdDate).toLocaleDateString()}
              </CardDescription>
            )}
          </CardHeader>
          <CardContent>
            {workspace.ownerId && (
              <p className="text-sm text-gray-500">
                Владелец: {workspace.ownerId}
              </p>
            )}
          </CardContent>
        </Card>
          );
        })()
      ))}
      
      <CreateCard 
        title="Создать рабочую область" 
        onClick={() => setIsModalOpen(true)}
      />

      <CreateWorkspaceModal 
        isOpen={isModalOpen}
        onOpenChange={setIsModalOpen}
        onSuccess={handleWorkspaceCreated}
      />

      <Dialog open={!!workspaceToDelete} onOpenChange={(open) => !open && setWorkspaceToDelete(null)}>
        <DialogContent className="sm:max-w-md bg-white text-black border-gray-700">
          <DialogHeader>
            <DialogTitle>Подтвердите удаление</DialogTitle>
            <DialogDescription>
              {workspaceToDelete
                ? `Вы действительно хотите удалить рабочую область "${workspaceToDelete.name}"?`
                : ""}
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