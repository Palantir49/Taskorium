import { useState } from "react";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogClose } from "./ui/dialog";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { Label } from "./ui/label";
import { createProject } from "../api/projectService";
import { CreateProjectRequest } from "../types/project";

interface CreateProjectModalProps {
  isOpen: boolean;
  onOpenChange: (open: boolean) => void;
  workspaceId?: string;
  onSuccess?: (project: any) => void;
}

export default function CreateProjectModal({ 
  isOpen, 
  onOpenChange, 
  workspaceId,
  onSuccess 
}: CreateProjectModalProps) {
  const [projectName, setProjectName] = useState("");
  const [projectDescription, setProjectDescription] = useState("");
  const [projectAbbreviation, setProjectAbbreviation] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!projectName.trim() || !projectAbbreviation.trim() || !workspaceId) {
      setError("Заполните все обязательные поля");
      return;
    }

    setIsLoading(true);
    setError(null);

    try {
      const requestData: CreateProjectRequest = {
        name: projectName.trim(),
        description: projectDescription.trim(),
        abbreviation: projectAbbreviation.trim(),
        workspaceId: workspaceId
      };

      const newProject = await createProject(requestData);
      
      setProjectName("");
      setProjectDescription("");
      setProjectAbbreviation("");
      onOpenChange(false);
      
      if (onSuccess) {
        onSuccess(newProject);
      }
    } catch (err) {
      setError("Ошибка при создании проекта");
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-md bg-white text-black border-gray-700">
        <DialogHeader>
          <DialogTitle>Создать проект</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="projectName">Название</Label>
            <Input
              id="projectName"
              value={projectName}
              onChange={(e) => setProjectName(e.target.value)}
              placeholder="Введите название проекта"
              disabled={isLoading}
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="projectDescription">Описание (опционально)</Label>
            <Input
              id="projectDescription"
              value={projectDescription}
              onChange={(e) => setProjectDescription(e.target.value)}
              placeholder="Введите описание проекта"
              disabled={isLoading}
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="projectAbbreviation">Аббревиатура</Label>
            <Input
              id="projectAbbreviation"
              value={projectAbbreviation}
              onChange={(e) => setProjectAbbreviation(e.target.value)}
              placeholder="Введите аббревиатуру"
              disabled={isLoading}
            />
          </div>
          
          {error && (
            <div className="text-sm text-red-600 bg-red-50 px-3 py-2 rounded-md">
              {error}
            </div>
          )}

          <div className="flex justify-end space-x-2 pt-4">
            <DialogClose asChild>
              <Button type="button" variant="secondary" disabled={isLoading}>
                Отмена
              </Button>
            </DialogClose>
            <Button 
              type="submit" 
              disabled={isLoading || !projectName.trim() || !projectAbbreviation.trim() || !workspaceId}
            >
              {isLoading ? "Создание..." : "Создать"}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}