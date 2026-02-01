import { Card, CardContent, CardHeader, CardDescription, CardTitle } from "./ui/card";

const workspaceData = [
  {
    title: "Разработка",
    description: "Frontend и backend проекты, код-ревью"
  },
  {
    title: "Дизайн",
    description: "UI/UX, прототипы, графические материалы"
  },
  {
    title: "Аналитика",
    description: "Метрики, отчеты, дашборды"
  }
];

export default function WorkspaceCard() {
  return (
    <>
      {workspaceData.map((workspace, index) => (
        <Card className="border-gray-300 mx-auto w-full max-w-sm">
          <CardHeader>
            <CardTitle className="text-xl">{workspace.title}</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-gray-600">{workspace.description}</p>
          </CardContent>
        </Card>
      ))}
    </>
  );
}