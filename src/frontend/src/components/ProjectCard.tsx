import { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";
import { Badge } from "./ui/badge";
import CreateCard from "./CreateCard";
import CreateProjectModal from "./CreateProjectModal";
import { fetchProjectsByWorkspaceId } from "../api/projectService";
import { ProjectResponse } from "../types/project";

interface ProjectCardProps {
  workspaceId?: string;
  onSelect?: (projectId: string) => void;
}

export default function ProjectCard({ workspaceId, onSelect }: ProjectCardProps) {
  const [projects, setProjects] = useState<ProjectResponse[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    if (!workspaceId) {
      setProjects([]);
      return;
    }

    const loadProjects = async () => {
      const data = await fetchProjectsByWorkspaceId(workspaceId);
      setProjects(data);
    };

    loadProjects();
  }, [workspaceId]);

  const handleProjectCreated = (newProject: ProjectResponse) => {
    setProjects([...projects, newProject]);
  };

  return (
    <>
      {projects.map((project) => (
        <Card key={project.id} className="border-gray-300" onClick={() => onSelect?.(project.id)}>
          <CardHeader className="pb-3">
            <div className="flex justify-between items-start">
              <CardTitle className="text-lg">{project.name}</CardTitle>
              <Badge variant="secondary">
                В работе
              </Badge>
            </div>
          </CardHeader>
          <CardContent className="space-y-4">
            {project.description && (
              <div className="text-sm text-gray-600">
                {project.description}
              </div>
            )}
            <div className="text-sm text-gray-600">
              Аббревиатура: <span className="font-semibold">{project.abbreviation}</span>
            </div>
          </CardContent>
        </Card>
      ))}
      
      {workspaceId && (
        <CreateCard 
          title="Создать проект" 
          onClick={() => setIsModalOpen(true)}
        />
      )}

      {workspaceId && (
        <CreateProjectModal 
          isOpen={isModalOpen}
          onOpenChange={setIsModalOpen}
          workspaceId={workspaceId}
          onSuccess={handleProjectCreated}
        />
      )}

      {!workspaceId && (
        <div className="text-center text-gray-500 py-8">
          Выберите рабочую область для создания проектов
        </div>
      )}
    </>
  );
}