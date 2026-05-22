import { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";
import { Badge } from "./ui/badge";
import { Button } from "./ui/button";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription, DialogFooter } from "./ui/dialog";
import CreateCard from "./CreateCard";
import CreateProjectModal from "./CreateProjectModal";
import { fetchProjectsByWorkspaceId, deleteProject } from "../api/projectService";
import { ProjectResponse } from "../types/project";

interface ProjectCardProps {
  workspaceId?: string;
  onSelect?: (projectId: string) => void;
}

export default function ProjectCard({ workspaceId, onSelect }: ProjectCardProps) {
  const [projects, setProjects] = useState<ProjectResponse[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [projectToDelete, setProjectToDelete] = useState<ProjectResponse | null>(null);

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

  const handleProjectDelete = async () => {
    if (!projectToDelete) return;
    try {
      await deleteProject(projectToDelete.id);
      setProjects((prev) => prev.filter((project) => project.id !== projectToDelete.id));
      setProjectToDelete(null);
    } catch (error) {
      console.error("Ошибка удаления проекта:", error);
      alert("Не удалось удалить проект");
    }
  };

  return (
    <>
      {projects.map((project) => (
        <Card key={project.id} className="relative border-gray-300" onClick={() => onSelect?.(project.id)}>
          <button
            type="button"
            className="absolute right-3 top-3 z-10 text-gray-500 hover:text-red-600"
            title="Удалить проект"
            onClick={(e) => {
              e.stopPropagation();
              setProjectToDelete(project);
            }}
          >
            🗑️
          </button>
          <CardHeader className="pb-3">
            <div className="flex justify-between items-start">
              <CardTitle className="text-lg">{project.name}</CardTitle>
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

      <Dialog open={!!projectToDelete} onOpenChange={(open) => !open && setProjectToDelete(null)}>
        <DialogContent className="sm:max-w-md bg-white text-black border-gray-700">
          <DialogHeader>
            <DialogTitle>Подтвердите удаление</DialogTitle>
            <DialogDescription>
              {projectToDelete
                ? `Вы действительно хотите удалить проект "${projectToDelete.name}"?`
                : ""}
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