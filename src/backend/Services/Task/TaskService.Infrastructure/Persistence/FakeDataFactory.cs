using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Persistence
{
    public static class FakeDataFactory
    {
        public static List<User> Users { get; set; } = new List<User>()
        {

        };
        public static List<Workspace> Workspaces { get; set; } = new List<Workspace>();
        public static List<WorkspaceMember> WorkspaceMembers { get; set; } = new List<WorkspaceMember>();
        public static List<Project> Projects { get; set; } = new List<Project>();
        public static List<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public static List<Issue> Issues { get; set; } = new List<Issue>();
        public static List<IssueStatus> IssueStatuses { get; set; } = new List<IssueStatus>();
        public static List<IssueType> IssueTypes { get; set; } = new List<IssueType>();
        static FakeDataFactory()
        {
            #region Users
            var vanya = User.Create(keycloakId: Guid.Parse("10000000-0000-0000-0000-000000000000"),
                                     userName: new UserName("ivan2004"),
                                     email: new EmailAdress("vanya2004@gmail.com"),
                                     fullName: "Иван Иванов");
            var petya = User.Create(keycloakId: Guid.Parse("20000000-0000-0000-0000-000000000000"),
                                     userName: new UserName("petya1337"),
                                     email: new EmailAdress("pyotr@gmail.com"),
                                     fullName: "Петя Петров");
            var kirill = User.Create(keycloakId: Guid.Parse("30000000-0000-0000-0000-000000000000"),
                                     userName: new UserName("b0kov_k1r1ll"),
                                     email: new EmailAdress("kirill_bokov@bk.ru"),
                                     fullName: "Кирилл Боков");
            var alexey = User.Create(keycloakId: Guid.Parse("30000000-0000-0000-0000-000000000000"),
                                     userName: new UserName("alexey1"),
                                     email: new EmailAdress("nikitin_alexey2001@gmail.com"),
                                     fullName: "Алексей Никитин");
            #endregion

            #region Workspaces
            var workspace1 = Workspace.Create(name: "Рабочая область 1",
                                                    ownerId: vanya.Id);
            var taskoriumWorkspace = Workspace.Create(name: "Рабочая область 2",
                                                    ownerId: vanya.Id);
            var workspace3 = Workspace.Create(name: "Рабочая область 3",
                                                    ownerId: vanya.Id);
            #endregion

            #region WorkspaceMembers
            var workspace1Member1 = WorkspaceMember.Create(workspaceId: workspace1.Id,
                                                           userId: petya.Id,
                                                           role: Roles.Member,
                                                           DateTimeOffset.UtcNow);
            var workspace1Member2 = WorkspaceMember.Create(workspaceId: workspace1.Id,
                                                           userId: vanya.Id,
                                                           role: Roles.Admin,
                                                           DateTimeOffset.UtcNow);
            var taskoriumWorkspaceMember1 = WorkspaceMember.Create(workspaceId: taskoriumWorkspace.Id,
                                                                   userId: petya.Id,
                                                                   role: Roles.Member,
                                                                   DateTimeOffset.UtcNow);
            var taskoriumWorkspaceMember2 = WorkspaceMember.Create(workspaceId: taskoriumWorkspace.Id,
                                                                   userId: kirill.Id,
                                                                   role: Roles.Admin,
                                                                   DateTimeOffset.UtcNow);
            var taskoriumWorkspaceMember3 = WorkspaceMember.Create(workspaceId: taskoriumWorkspace.Id,
                                                                   userId: alexey.Id,
                                                                   role: Roles.Viewer,
                                                                   DateTimeOffset.UtcNow);
            var workspace3Member1 = WorkspaceMember.Create(workspaceId: workspace3.Id,
                                                           userId: alexey.Id,
                                                           role: Roles.Admin,
                                                           DateTimeOffset.UtcNow);
            #endregion

            #region Projects
            var backendService = Project.Create(name: "Taskorium backend service",
                                              description: "Backend нашего командного проекта, который мы делаем в рамках курса OTUS",
                                              workspaceId: taskoriumWorkspace.Id);
            var openSource = Project.Create(name: "Open source библиотека React компонентов",
                                              description: "Библиотека готовых React компонентов с открытым исходным кодом от команды TaskForce",
                                              workspaceId: taskoriumWorkspace.Id);
            var notificationService = Project.Create(name: "Taskorium notification service",
                                              description: "Микросервис отправки уведомлений для Taskroium",
                                              workspaceId: taskoriumWorkspace.Id);
            var workspace1project1 = Project.Create(name: "Первый проект рабочей области",
                                              description: "",
                                              workspaceId: workspace1.Id);
            var workspace1project2 = Project.Create(name: "Внедрение ИИ в workflow разработки команды",
                                              description: "Какое то описание",
                                              workspaceId: workspace1.Id);
            var workspace3project1 = Project.Create(name: "Какой то проект",
                                              description: "Этот проект никто не любит",
                                              workspaceId: workspace3.Id);
            var workspace3project2 = Project.Create(name: "Еще один непонятный проект",
                                              description: "Но он всем нравится",
                                              workspaceId: workspace3.Id);
            #endregion

            #region ProjectMembers
            var backendServiceMember = ProjectMember.Create(projectId: backendService.Id,
                                                                      userId: petya.Id,
                                                                      role: Roles.Member,
                                                                      joinedAt: DateTimeOffset.UtcNow);
            var backendServiceMember1 = ProjectMember.Create(projectId: backendService.Id,
                                                                       userId: kirill.Id,
                                                                       role: Roles.Admin,
                                                                       joinedAt: DateTimeOffset.UtcNow);
            var backendServiceMember2 = ProjectMember.Create(projectId: backendService.Id,
                                                                       userId: alexey.Id,
                                                                       role: Roles.Member,
                                                                       joinedAt: DateTimeOffset.UtcNow);
            var openSourceMember = ProjectMember.Create(projectId: openSource.Id,
                                                                  userId: petya.Id,
                                                                  role: Roles.Member,
                                                                  joinedAt: DateTimeOffset.UtcNow);
            var openSourceMember1 = ProjectMember.Create(projectId: openSource.Id,
                                                                   userId: kirill.Id,
                                                                   role: Roles.Admin,
                                                                   joinedAt: DateTimeOffset.UtcNow);
            var norificationServiceMember = ProjectMember.Create(projectId: notificationService.Id,
                                                                           userId: alexey.Id,
                                                                           role: Roles.Admin,
                                                                           joinedAt: DateTimeOffset.UtcNow);
            var norificationServiceMember2 = ProjectMember.Create(projectId: notificationService.Id,
                                                                            userId: alexey.Id,
                                                                            role: Roles.Member,
                                                                            joinedAt: DateTimeOffset.UtcNow);

            #endregion

            #region Issues
            #endregion
            #region IssueStatuses
            #endregion
            #region IssueTypes
            #endregion


        }
    }
}
