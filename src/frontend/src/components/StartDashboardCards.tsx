import WorkspaceCard from "./WorkspaceCard";
import ProjectCard from "./ProjectCard.tsx";

export default function StartDashboardCard() {
  return (
    <div className="min-h-screen p-8">
      <h1 className="text-3xl font-bold mb-8">Рабочие пространства</h1>
      
      {/* Первая область */}
      <div className="border-2 border-gray-200 rounded-xl p-6 mb-8">
        <h2 className="text-2xl font-bold mb-4">Рабочие области</h2>
        <div className="grid grid-cols-3 gap-6">
          <WorkspaceCard />
        </div>
      </div>

      {/* Вторая область */}
      <div className="border-2 border-gray-200 rounded-xl p-6 mb-8">
        <h2 className="text-xl font-semibold mb-4">Проекты</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <ProjectCard />
        </div>
      </div>
    </div>
  );
}