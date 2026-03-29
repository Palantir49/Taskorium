using System;
using System.Collections.Generic;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Persistence
{
    /// <summary>
    /// One row in the Issue ↔ Tag many-to-many join table (<c>IssueTag</c>), matching EF column names <c>IssuesId</c> / <c>TagsId</c>.
    /// </summary>
    public sealed record IssueTag(Guid IssuesId, Guid TagsId);

    /// <summary>
    /// Sample data for local development and future DB seeding. Creation order follows foreign keys from entity configurations
    /// (Workspace → User owner; Project → Workspace; IssueStatus / Tag → Project; Issue → Project + IssueStatus;
    /// ProjectMember / WorkspaceMember composite keys; Attachment → Issue + User; IssueTag → Issue + Tag).
    /// </summary>
    public static class FakeDataFactory
    {
        static FakeDataFactory()
        {
            Seed();
        }

        public static List<User> Users { get; private set; } = null!;
        public static List<Workspace> Workspaces { get; private set; } = null!;
        public static List<WorkspaceMember> WorkspaceMembers { get; private set; } = null!;
        public static List<Project> Projects { get; private set; } = null!;
        public static List<ProjectMember> ProjectMembers { get; private set; } = null!;
        public static List<IssueStatus> IssueStatuses { get; private set; } = null!;
        public static List<Tag> Tags { get; private set; } = null!;
        public static List<Issue> Issues { get; private set; } = null!;
        public static List<IssueTag> IssueTags { get; private set; } = null!;
        public static List<Attachment> Attachments { get; private set; } = null!;

        private static void Seed()
        {
            var joined = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

            var alice = User.Create(
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                new UserName("alice"),
                new EmailAdress("alice@example.com"),
                "Alice Anderson");
            var bob = User.Create(
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                new UserName("bob"),
                new EmailAdress("bob@example.com"),
                "Bob Brown");
            var carol = User.Create(
                Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                new UserName("carol"),
                new EmailAdress("carol@example.com"),
                "Carol Chen");

            Users = new List<User> { alice, bob, carol };

            var workspace = Workspace.Create("Acme Workspace");
            Workspaces = new List<Workspace> { workspace };

            WorkspaceMembers = new List<WorkspaceMember>
            {
                WorkspaceMember.Create(workspace.Id, alice.Id, Roles.Creator, joined),
                WorkspaceMember.Create(workspace.Id, bob.Id, Roles.Admin, joined.AddDays(1)),
                WorkspaceMember.Create(workspace.Id, carol.Id, Roles.Member, joined.AddDays(2))
            };

            var project = Project.Create(
                "Mobile client",
                "Customer-facing iOS and Android app.",
                "MOB",
                workspace.Id);
            Projects = new List<Project> { project };

            ProjectMembers = new List<ProjectMember>
            {
                ProjectMember.Create(project.Id, alice.Id, Roles.Creator, joined),
                ProjectMember.Create(project.Id, bob.Id, Roles.Member, joined.AddDays(1)),
                ProjectMember.Create(project.Id, carol.Id, Roles.Viewer, joined.AddDays(3))
            };

            var statusBacklog = IssueStatus.Create("Backlog", (int)IssueStatusType.Initial, 0, project.Id);
            var statusInProgress = IssueStatus.Create("In progress", (int)IssueStatusType.Process, 1, project.Id);
            var statusDone = IssueStatus.Create("Done", (int)IssueStatusType.Success, 2, project.Id);
            IssueStatuses = new List<IssueStatus> { statusBacklog, statusInProgress, statusDone };

            var tagFrontend = Tag.Create("frontend", project.Id);
            var tagBackend = Tag.Create("backend", project.Id);
            var tagBug = Tag.Create("bug", project.Id);
            Tags = new List<Tag> { tagFrontend, tagBackend, tagBug };

            var issue1 = Issue.Create(
                "Login screen",
                "Implement email and SSO login.",
                "MOB-1",
                project.Id,
                statusInProgress.Id,
                (int)IssueType.Task,
                (int)IssuePriority.High,
                new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero));
            var issue2 = Issue.Create(
                "Crash on cold start",
                "Reproduces on Android 14.",
                "MOB-2",
                project.Id,
                statusBacklog.Id,
                (int)IssueType.Bug,
                (int)IssuePriority.Critical,
                new DateTimeOffset(2026, 2, 20, 0, 0, 0, TimeSpan.Zero));
            var issue3 = Issue.Create(
                "Push notification copy",
                "Finalize strings for v1.",
                "MOB-3",
                project.Id,
                statusDone.Id,
                (int)IssueType.Story,
                (int)IssuePriority.Low,
                null);
            Issues = new List<Issue> { issue1, issue2, issue3 };

            IssueTags = new List<IssueTag>
            {
                new(issue1.Id, tagFrontend.Id),
                new(issue1.Id, tagBackend.Id),
                new(issue2.Id, tagBug.Id),
                new(issue2.Id, tagBackend.Id),
                new(issue3.Id, tagFrontend.Id)
            };

            Attachments = new List<Attachment>
            {
                Attachment.Create(issue1.Id, alice.Id, "attachments/mob-1/wireframe.png"),
                Attachment.Create(issue2.Id, bob.Id, "attachments/mob-2/logcat.txt")
            };
        }
    }
}
