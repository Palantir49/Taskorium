using System;
using System.Collections.Generic;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Persistence
{
    public static class FakeDataFactory
    {
        public static List<User> Users { get; set; } = new List<User>();
        public static List<Workspace> Workspaces { get; set; } = new List<Workspace>();
        public static List<WorkspaceMember> WorkspaceMembers { get; set; } = new List<WorkspaceMember>();
        public static List<Project> Projects { get; set; } = new List<Project>();
        public static List<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public static List<Issue> Issues { get; set; } = new List<Issue>();
        public static List<IssueStatus> IssueStatuses { get; set; } = new List<IssueStatus>();
        public static List<IssueType> IssueTypes { get; set; } = new List<IssueType>();
        public static List<Attachment> Attachments { get; set; } = new List<Attachment>();

        static FakeDataFactory()
        {
            var seedJoinedAt = new DateTimeOffset(2025, 1, 8, 9, 0, 0, TimeSpan.Zero);

            #region Users
            var ivan = User.Create(
                keycloakId: Guid.Parse("10000000-0000-0000-0000-000000000001"),
                userName: new UserName("ivan.petrov"),
                email: new EmailAdress("ivan.petrov@taskorium.local"),
                fullName: "Ivan Petrov");
            var peter = User.Create(
                keycloakId: Guid.Parse("20000000-0000-0000-0000-000000000002"),
                userName: new UserName("peter.sokolov"),
                email: new EmailAdress("peter.sokolov@taskorium.local"),
                fullName: "Peter Sokolov");
            var kirill = User.Create(
                keycloakId: Guid.Parse("30000000-0000-0000-0000-000000000003"),
                userName: new UserName("kirill.volkov"),
                email: new EmailAdress("kirill.volkov@taskorium.local"),
                fullName: "Kirill Volkov");
            var alexey = User.Create(
                keycloakId: Guid.Parse("40000000-0000-0000-0000-000000000004"),
                userName: new UserName("alexey.nikitin"),
                email: new EmailAdress("alexey.nikitin@taskorium.local"),
                fullName: "Alexey Nikitin");
            #endregion

            #region Workspaces
            var personalWorkspace = Workspace.Create(
                name: "Personal",
                ownerId: ivan.Id);
            var taskoriumTeam = Workspace.Create(
                name: "Taskorium",
                ownerId: ivan.Id);
            var sandboxWorkspace = Workspace.Create(
                name: "Sandbox",
                ownerId: ivan.Id);
            #endregion

            #region WorkspaceMembers
            var personalMemberIvan = WorkspaceMember.Create(
                workspaceId: personalWorkspace.Id,
                userId: ivan.Id,
                role: Roles.Admin,
                joinedAt: seedJoinedAt);
            var personalMemberPeter = WorkspaceMember.Create(
                workspaceId: personalWorkspace.Id,
                userId: peter.Id,
                role: Roles.Member,
                joinedAt: seedJoinedAt);
            var taskoriumMemberIvan = WorkspaceMember.Create(
                workspaceId: taskoriumTeam.Id,
                userId: ivan.Id,
                role: Roles.Admin,
                joinedAt: seedJoinedAt);
            var taskoriumMemberPeter = WorkspaceMember.Create(
                workspaceId: taskoriumTeam.Id,
                userId: peter.Id,
                role: Roles.Member,
                joinedAt: seedJoinedAt);
            var taskoriumMemberKirill = WorkspaceMember.Create(
                workspaceId: taskoriumTeam.Id,
                userId: kirill.Id,
                role: Roles.Admin,
                joinedAt: seedJoinedAt);
            var taskoriumMemberAlexey = WorkspaceMember.Create(
                workspaceId: taskoriumTeam.Id,
                userId: alexey.Id,
                role: Roles.Viewer,
                joinedAt: seedJoinedAt);
            var sandboxMemberAlexey = WorkspaceMember.Create(
                workspaceId: sandboxWorkspace.Id,
                userId: alexey.Id,
                role: Roles.Admin,
                joinedAt: seedJoinedAt);
            #endregion

            #region Projects
            var backendService = Project.Create(
                name: "taskorium-api",
                description: "ASP.NET Core service for workspaces, projects, issues, and notifications. Exposes REST APIs consumed by the web client.",
                workspaceId: taskoriumTeam.Id);
            var openSource = Project.Create(
                name: "ui-kit",
                description: "Open-source React component library shared across Taskorium frontends: layout primitives, forms, and accessibility-tested patterns.",
                workspaceId: taskoriumTeam.Id);
            var notificationService = Project.Create(
                name: "taskorium-notify",
                description: "Outbox-backed worker that delivers email and in-app notifications with retry policies and idempotent handlers.",
                workspaceId: taskoriumTeam.Id);
            var aiWorkflowProject = Project.Create(
                name: "ai-dev-workflow",
                description: "Pilot: lint prompts, CI gates, and review checklists for teams adopting AI-assisted coding without bypassing quality bars.",
                workspaceId: personalWorkspace.Id);
            var docsRefresh = Project.Create(
                name: "docs-refresh",
                description: "Rewrite onboarding and API reference for the next major release; align examples with current OpenAPI spec.",
                workspaceId: personalWorkspace.Id);
            var legacyCleanup = Project.Create(
                name: "legacy-cleanup",
                description: "Track removal of deprecated endpoints and migration of remaining consumers to v2 APIs.",
                workspaceId: sandboxWorkspace.Id);
            var spikePrototype = Project.Create(
                name: "spike-realtime-board",
                description: "Time-boxed prototype: WebSocket presence and optimistic updates for the board view.",
                workspaceId: sandboxWorkspace.Id);
            #endregion

            #region ProjectMembers
            var backendMemberPeter = ProjectMember.Create(
                projectId: backendService.Id,
                userId: peter.Id,
                role: Roles.Member,
                joinedAt: seedJoinedAt);
            var backendMemberKirill = ProjectMember.Create(
                projectId: backendService.Id,
                userId: kirill.Id,
                role: Roles.Admin,
                joinedAt: seedJoinedAt);
            var backendMemberAlexey = ProjectMember.Create(
                projectId: backendService.Id,
                userId: alexey.Id,
                role: Roles.Member,
                joinedAt: seedJoinedAt);
            var openSourcePeter = ProjectMember.Create(
                projectId: openSource.Id,
                userId: peter.Id,
                role: Roles.Member,
                joinedAt: seedJoinedAt);
            var openSourceKirill = ProjectMember.Create(
                projectId: openSource.Id,
                userId: kirill.Id,
                role: Roles.Admin,
                joinedAt: seedJoinedAt);
            var notifyAlexey = ProjectMember.Create(
                projectId: notificationService.Id,
                userId: alexey.Id,
                role: Roles.Admin,
                joinedAt: seedJoinedAt);
            var notifyPeter = ProjectMember.Create(
                projectId: notificationService.Id,
                userId: peter.Id,
                role: Roles.Member,
                joinedAt: seedJoinedAt);
            #endregion

            #region IssueStatuses (taskorium-api)
            var apiTodo = IssueStatus.Create(
                name: "To Do",
                type: nameof(IssueStatusType.Initial),
                position: 0,
                color: "#94A3B8",
                projectId: backendService.Id);
            var apiInProgress = IssueStatus.Create(
                name: "In Progress",
                type: nameof(IssueStatusType.Process),
                position: 1,
                color: "#3B82F6",
                projectId: backendService.Id);
            var apiInReview = IssueStatus.Create(
                name: "In Review",
                type: nameof(IssueStatusType.Process),
                position: 2,
                color: "#A855F7",
                projectId: backendService.Id);
            var apiDone = IssueStatus.Create(
                name: "Done",
                type: nameof(IssueStatusType.Success),
                position: 3,
                color: "#22C55E",
                projectId: backendService.Id);
            #endregion

            #region IssueTypes (taskorium-api)
            var apiTypeBug = IssueType.Create(
                name: "Bug",
                projectId: backendService.Id,
                color: "#EF4444");
            var apiTypeFeature = IssueType.Create(
                name: "Feature",
                projectId: backendService.Id,
                color: "#0EA5E9");
            var apiTypeChore = IssueType.Create(
                name: "Chore",
                projectId: backendService.Id,
                color: "#64748B");
            #endregion

            #region IssueStatuses (ui-kit)
            var uiBacklog = IssueStatus.Create(
                name: "Backlog",
                type: nameof(IssueStatusType.Initial),
                position: 0,
                color: "#94A3B8",
                projectId: openSource.Id);
            var uiDone = IssueStatus.Create(
                name: "Released",
                type: nameof(IssueStatusType.Success),
                position: 1,
                color: "#22C55E",
                projectId: openSource.Id);
            #endregion

            #region IssueTypes (ui-kit)
            var uiTypeTask = IssueType.Create(
                name: "Task",
                projectId: openSource.Id,
                color: "#6366F1");
            var uiTypeBug = IssueType.Create(
                name: "Bug",
                projectId: openSource.Id,
                color: "#F97316");
            #endregion

            #region IssueStatuses (taskorium-notify)
            var notifyTodo = IssueStatus.Create(
                name: "To Do",
                type: nameof(IssueStatusType.Initial),
                position: 0,
                color: "#94A3B8",
                projectId: notificationService.Id);
            var notifyDone = IssueStatus.Create(
                name: "Done",
                type: nameof(IssueStatusType.Success),
                position: 1,
                color: "#22C55E",
                projectId: notificationService.Id);
            #endregion

            #region IssueTypes (taskorium-notify)
            var notifyTypeTask = IssueType.Create(
                name: "Task",
                projectId: notificationService.Id,
                color: "#0EA5E9");
            #endregion

            #region IssueStatuses (ai-dev-workflow)
            var aiTodo = IssueStatus.Create(
                name: "To Do",
                type: nameof(IssueStatusType.Initial),
                position: 0,
                color: "#94A3B8",
                projectId: aiWorkflowProject.Id);
            var aiDone = IssueStatus.Create(
                name: "Done",
                type: nameof(IssueStatusType.Success),
                position: 1,
                color: "#22C55E",
                projectId: aiWorkflowProject.Id);
            #endregion

            #region IssueTypes (ai-dev-workflow)
            var aiTypeTask = IssueType.Create(
                name: "Task",
                projectId: aiWorkflowProject.Id,
                color: "#64748B");
            #endregion

            #region Issues
            var issueApiOpenapi = Issue.Create(
                name: "Publish OpenAPI 3.1 bundle for staging",
                description: "Generate and upload the merged spec to the docs bucket; add link from the repo README.",
                projectId: backendService.Id,
                taskTypeId: apiTypeChore.Id,
                taskStatusId: apiInReview.Id,
                dueDate: new DateTimeOffset(2025, 3, 28, 17, 0, 0, TimeSpan.Zero));
            var issueApiPagination = Issue.Create(
                name: "Fix inconsistent cursor encoding on issue list",
                description: "Repro: list issues with page size 50; next cursor sometimes returns 400. Add regression test against snapshot payloads.",
                projectId: backendService.Id,
                taskTypeId: apiTypeBug.Id,
                taskStatusId: apiInProgress.Id,
                dueDate: new DateTimeOffset(2025, 3, 22, 12, 0, 0, TimeSpan.Zero));
            var issueApiWebhooks = Issue.Create(
                name: "Webhook delivery retries with exponential backoff",
                description: "Align with product spec: max 8 attempts, jitter, dead-letter after final failure.",
                projectId: backendService.Id,
                taskTypeId: apiTypeFeature.Id,
                taskStatusId: apiTodo.Id,
                dueDate: new DateTimeOffset(2025, 4, 5, 0, 0, 0, TimeSpan.Zero));
            var issueApiDone = Issue.Create(
                name: "Health checks return dependency status",
                description: "Expose SQL and message bus connectivity in /health/detailed for k8s probes.",
                projectId: backendService.Id,
                taskTypeId: apiTypeFeature.Id,
                taskStatusId: apiDone.Id,
                dueDate: null);

            var issueUiFocus = Issue.Create(
                name: "Focus trap breaks inside nested Dialog",
                description: "NVDA + Chrome: tab cycle escapes modal when DatePicker overlay opens. Repro steps attached to design QA doc.",
                projectId: openSource.Id,
                taskTypeId: uiTypeBug.Id,
                taskStatusId: uiBacklog.Id,
                dueDate: new DateTimeOffset(2025, 3, 30, 0, 0, 0, TimeSpan.Zero));
            var issueUiTokens = Issue.Create(
                name: "Ship semantic color tokens for dark mode",
                description: "Map legacy palette to new token names; no visual change in light theme.",
                projectId: openSource.Id,
                taskTypeId: uiTypeTask.Id,
                taskStatusId: uiDone.Id,
                dueDate: null);

            var issueNotifySmtp = Issue.Create(
                name: "Rotate SMTP credentials in vault",
                description: "Vendor scheduled March maintenance; update sealed secrets and verify staging send.",
                projectId: notificationService.Id,
                taskTypeId: notifyTypeTask.Id,
                taskStatusId: notifyTodo.Id,
                dueDate: new DateTimeOffset(2025, 3, 25, 8, 0, 0, TimeSpan.Zero));

            var issueAiGuidelines = Issue.Create(
                name: "Draft team guidelines for AI-generated PR descriptions",
                description: "One page: required sections, tone, and when to require human rewrite before merge.",
                projectId: aiWorkflowProject.Id,
                taskTypeId: aiTypeTask.Id,
                taskStatusId: aiTodo.Id,
                dueDate: new DateTimeOffset(2025, 4, 1, 0, 0, 0, TimeSpan.Zero));
            #endregion

            #region Attachments
            var attachmentSpec = Attachment.Create(
                issueId: issueApiOpenapi.Id,
                uploaderId: kirill.Id,
                storagePath: "issues/taskorium-api/openapi-staging/checklist.pdf");
            var attachmentUiRepro = Attachment.Create(
                issueId: issueUiFocus.Id,
                uploaderId: peter.Id,
                storagePath: "issues/ui-kit/dialog-focus/nested-dialog-repro.webm");
            #endregion

            Users.AddRange(ivan, peter, kirill, alexey);
            Workspaces.AddRange(personalWorkspace, taskoriumTeam, sandboxWorkspace);
            WorkspaceMembers.AddRange(
                personalMemberIvan,
                personalMemberPeter,
                taskoriumMemberIvan,
                taskoriumMemberPeter,
                taskoriumMemberKirill,
                taskoriumMemberAlexey,
                sandboxMemberAlexey);
            Projects.AddRange(
                backendService,
                openSource,
                notificationService,
                aiWorkflowProject,
                docsRefresh,
                legacyCleanup,
                spikePrototype);
            ProjectMembers.AddRange(
                backendMemberPeter,
                backendMemberKirill,
                backendMemberAlexey,
                openSourcePeter,
                openSourceKirill,
                notifyAlexey,
                notifyPeter);
            IssueStatuses.AddRange(
                apiTodo,
                apiInProgress,
                apiInReview,
                apiDone,
                uiBacklog,
                uiDone,
                notifyTodo,
                notifyDone,
                aiTodo,
                aiDone);
            IssueTypes.AddRange(
                apiTypeBug,
                apiTypeFeature,
                apiTypeChore,
                uiTypeTask,
                uiTypeBug,
                notifyTypeTask,
                aiTypeTask);
            Issues.AddRange(
                issueApiOpenapi,
                issueApiPagination,
                issueApiWebhooks,
                issueApiDone,
                issueUiFocus,
                issueUiTokens,
                issueNotifySmtp,
                issueAiGuidelines);
            Attachments.AddRange(attachmentSpec, attachmentUiRepro);
        }
    }
}
