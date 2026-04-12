import { useState, useEffect } from "react";
import { Card, CardContent, CardHeader, CardDescription, CardTitle } from "./ui/card";
import CreateCard from "./CreateCard";
import CreateWorkspaceModal from "./CreateWorkspaceModal";
import { fetchUserWorkspaces } from "../api/workSpaceService";
import { WorkspaceResponse } from "../types/workspace";

interface WorkspaceCardProps {
  onSelect?: (workspaceId: string) => void;
}

export default function WorkspaceCard({ onSelect }: WorkspaceCardProps) {
  const [workspaces, setWorkspaces] = useState<WorkspaceResponse[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);

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

  return (
    <>
      {workspaces.map((workspace) => (
        <Card 
          key={workspace.id} 
          className="border-gray-300 mx-auto w-full max-w-sm cursor-pointer hover:border-blue-500 transition-colors"
          onClick={() => onSelect?.(workspace.id)}
        >
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
    </>
  );
}