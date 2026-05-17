import React, { useState } from "react";
import { useAuthContext } from "../providers/AuthProvider";
import WorkspaceCard from "./WorkspaceCard";
import ProjectCard from "./ProjectCard";
import DashboardTasks from "./DashboardTasks";
import { HeaderStartBoard } from "./HeaderStartBoard";

interface StartDashboardCardsProps {
  activeTab: string;
  onTabChange: React.Dispatch<React.SetStateAction<string>>;
  showHeader?: boolean;
}

export default function StartDashboardCard({ 
  activeTab, 
  onTabChange, 
  showHeader = true 
}: StartDashboardCardsProps) {
  const authInfo = useAuthContext();
  const [selectedProjectId, setSelectedProjectId] = useState<string | null>(null);
  const [selectedWorkspaceId, setSelectedWorkspaceId] = useState<string | null>(null);

  if (selectedProjectId) {
    return <DashboardTasks 
      activeTab={activeTab}
      onTabChange={onTabChange}
      showHeader={showHeader}
      projectId={selectedProjectId}
    />;
  }

  return (
    <div className="min-h-screen p-8">
      {showHeader && authInfo.isAuthenticated && (
        <HeaderStartBoard authInfo={authInfo} />
      )}

      <h1 className="text-3xl font-bold mb-8">Рабочие пространства</h1>
      
      {/* Первая область */}
      <div className="border-2 border-gray-200 rounded-xl p-6 mb-8">
        <h2 className="text-2xl font-bold mb-4">Рабочие области</h2>
        <div className="grid grid-cols-3 gap-6">
          <WorkspaceCard 
            selectedWorkspaceId={selectedWorkspaceId}
            onSelect={setSelectedWorkspaceId}
          />
        </div>
      </div>

      {/* Вторая область */}
      <div className="border-2 border-gray-200 rounded-xl p-6 mb-8">
        <h2 className="text-xl font-semibold mb-4">Проекты</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <ProjectCard 
            workspaceId={selectedWorkspaceId || undefined}
            onSelect={setSelectedProjectId}
          />
        </div>
      </div>
    </div>
  );
}