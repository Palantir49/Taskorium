import { useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";
import { Badge } from "./ui/badge";
import CreateCard from "./CreateCard";
import CreateProjectModal from "./CreateProjectModal";
import { ProjectResponse } from "../types/project";

// Замоканый проект для тестирования
const mockProject: ProjectResponse = {
  id: "mock-1",
  name: "Замоканный проект",
  description: "Мок",
  abbreviation: "TEST",
  workspaceId: "mock-workspace",
  createdDate: new Date().toISOString()
};

interface ProjectCardProps {
  workspaceId?: string;
  onSelect?: (id: number) => void;
}

export default function ProjectCard({ workspaceId, onSelect }: ProjectCardProps) {
  const [projects, setProjects] = useState<ProjectResponse[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleProjectCreated = (newProject: ProjectResponse) => {
    setProjects([...projects, newProject]);
  };

  return (
    <>
      {/* Замоканый проект для тестирования */}
      <Card 
        key={mockProject.id} 
        className="border-gray-300" 
        onClick={() => onSelect?.(0)}
      >
        <CardHeader className="pb-3">
          <div className="flex justify-between items-start">
            <CardTitle className="text-lg">{mockProject.name}</CardTitle>
            <Badge variant="secondary">В работе</Badge>
          </div>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="text-sm text-gray-600">
            {mockProject.description}
          </div>
          <div className="text-sm text-gray-600">
            Аббревиатура: <span className="font-semibold">{mockProject.abbreviation}</span>
          </div>
        </CardContent>
      </Card>

      {projects.map((project, index) => (
        <Card key={project.id} className="border-gray-300" onClick={() => onSelect?.(index + 1)}>
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
            <div className="text-sm text-gray-600">
              ID проекта: <span className="font-semibold">{project.id}</span>
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