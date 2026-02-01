import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";
import { Badge } from "./ui/badge";

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

export default function ProjectCard() {
  return (
    <>
      {projectData.map((project, index) => (
        <Card key={index} className="border-gray-300">
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