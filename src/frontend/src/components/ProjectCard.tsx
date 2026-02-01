import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";
import { Badge } from "./ui/badge";

interface ProjectCardProps {
  onSelect: (id: number) => void;
}

const projectData = [
  {
    title: "Веб-приложение",
    taskCount: 14,
    status: "В работе"
  },
  {
    title: "Мобильное приложение",
    taskCount: 8,
    status: "Завершен"
  }
];

export default function ProjectCard({ onSelect }: ProjectCardProps) {
  return (
    <>
      {projectData.map((project, index) => (
        <Card key={index} className="border-gray-300" onClick={() => onSelect(index)}>
          <CardHeader className="pb-3">
            <div className="flex justify-between items-start">
              <CardTitle className="text-lg">{project.title}</CardTitle>
              <Badge variant={project.status === "Завершен" ? "default" : "secondary"}>
                {project.status}
              </Badge>
            </div>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="text-sm text-gray-600">
              Задачи: <span className="font-semibold">{project.taskCount}</span>
            </div>
          </CardContent>
        </Card>
      ))}
    </>
  );
}