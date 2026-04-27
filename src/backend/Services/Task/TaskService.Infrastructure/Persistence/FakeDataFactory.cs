using System;
using System.Collections.Generic;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Persistence;

public static class FakeDataFactory
{
    public static IEnumerable<object> Users { get; private set; } = new[]
    {
    new
    {
        Id = Guid.Parse("019d58e9-98f4-7638-8fc0-f5e0a6809ec9"),
        Username = new UserName("test"),
        KeycloakId = Guid.Parse("e24e7bca-2ec4-4ba9-9106-18ba02272c93"),
        Email = new EmailAdress("test@test.ru"),
        CreatedDate = new DateTimeOffset(2026, 3, 20, 14, 30, 0, TimeSpan.Zero),
        FullName = "Test Testov"
    },
    new
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Username = new UserName("alice"),
        KeycloakId = Guid.Parse("k0000000-0000-0000-0000-000000000001"),
        Email = new EmailAdress("alice@example.com"),
        CreatedDate = new DateTimeOffset(2026, 3, 20, 14, 30, 0, TimeSpan.Zero),
        FullName = "Alice Anderson"
    },
    new
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Username = new UserName("bob"),
        KeycloakId = Guid.Parse("k0000000-0000-0000-0000-000000000002"),
        Email = new EmailAdress("bob@example.com"),
        CreatedDate = new DateTimeOffset(2026, 3, 20, 14, 30, 0, TimeSpan.Zero),
        FullName = "Bob Brown"
    },
    new
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
        Username = new UserName("carol"),
        KeycloakId = Guid.Parse("k0000000-0000-0000-0000-000000000003"),
        Email = new EmailAdress("carol@example.com"),
        CreatedDate = new DateTimeOffset(2026, 3, 20, 14, 30, 0, TimeSpan.Zero),
        FullName = "Carol Chen"
    },
};

    public static IEnumerable<object> Workspaces { get; private set; } = new[]
    {
    new
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Name = new BaseEntityName("Acme Corp"),
        CreatedDate = new DateTimeOffset(2026, 1, 15, 10, 0, 0, TimeSpan.Zero)
    },
    new
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Name = new BaseEntityName("Test Lab"),
        CreatedDate = new DateTimeOffset(2026, 3, 20, 14, 30, 0, TimeSpan.Zero)
    }
};

    public static IEnumerable<object> WorkspaceMembers { get; private set; } = null!;
    public static IEnumerable<object> Projects { get; private set; } = null!;
    public static IEnumerable<object> ProjectMembers { get; private set; } = null!;
    public static IEnumerable<object> IssueStatuses { get; private set; } = null!;
    public static IEnumerable<object> Tags { get; private set; } = null!;
    public static IEnumerable<object> Issues { get; private set; } = null!;
    public static IEnumerable<object> IssueTags { get; private set; } = null!;
    public static IEnumerable<object> Attachments { get; private set; } = null!;

    private static void Seed()
    {
        var joined = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

        // IDs для удобства повторного использования
        var aliceId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
        var bobId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2");
        var carolId = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc3");
        var workspaceId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var projectId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        // Workspaces
        Workspaces = new[]
        {
        new
        {
            Id = workspaceId,
            Name = new BaseEntityName("Acme Workspace"),
            CreatedDate = joined
        }
    };

        // WorkspaceMembers
        WorkspaceMembers = new[]
        {
        new { WorkspaceId = workspaceId, UserId = aliceId, Role = Roles.Creator, JoinedDate = joined },
        new { WorkspaceId = workspaceId, UserId = bobId, Role = Roles.Admin, JoinedDate = joined.AddDays(1) },
        new { WorkspaceId = workspaceId, UserId = carolId, Role = Roles.Member, JoinedDate = joined.AddDays(2) }
    };

        // Projects
        Projects = new[]
        {
        new
        {
            Id = projectId,
            Name = "Mobile client",
            Description = "Customer-facing iOS and Android app.",
            Key = "MOB",
            WorkspaceId = workspaceId,
            CreatedDate = joined
        }
    };

        // ProjectMembers
        ProjectMembers = new[]
        {
        new { ProjectId = projectId, UserId = aliceId, Role = Roles.Creator, JoinedDate = joined },
        new { ProjectId = projectId, UserId = bobId, Role = Roles.Member, JoinedDate = joined.AddDays(1) },
        new { ProjectId = projectId, UserId = carolId, Role = Roles.Viewer, JoinedDate = joined.AddDays(3) }
    };

        // IssueStatuses
        var statusBacklogId = Guid.NewGuid();
        var statusInProgressId = Guid.NewGuid();
        var statusDoneId = Guid.NewGuid();

        IssueStatuses = new[]
        {
        new { Id = statusBacklogId, Name = "Backlog", Type = IssueStatusType.Initial, Order = 0, ProjectId = projectId },
        new { Id = statusInProgressId, Name = "In progress", Type = IssueStatusType.Process, Order = 1, ProjectId = projectId },
        new { Id = statusDoneId, Name = "Done", Type = IssueStatusType.Success, Order = 2, ProjectId = projectId }
    };

        // Tags
        var tagFrontendId = Guid.NewGuid();
        var tagBackendId = Guid.NewGuid();
        var tagBugId = Guid.NewGuid();

        Tags = new[]
        {
        new { Id = tagFrontendId, Name = "frontend", ProjectId = projectId },
        new { Id = tagBackendId, Name = "backend", ProjectId = projectId },
        new { Id = tagBugId, Name = "bug", ProjectId = projectId }
    };

        // Issues
        var issue1Id = Guid.NewGuid();
        var issue2Id = Guid.NewGuid();
        var issue3Id = Guid.NewGuid();

        Issues = new[]
{
    new
    {
        Id = issue1Id,
        Title = "Login screen",
        Description = "Implement email and SSO login.",
        Key = "MOB-1",
        ProjectId = projectId,
        StatusId = statusInProgressId,
        Type = IssueType.Task,
        Priority = IssuePriority.High,
        DueDate = (DateTimeOffset?)new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero), // 👈 явное приведение
        CreatedDate = DateTimeOffset.UtcNow
    },
    new
    {
        Id = issue2Id,
        Title = "Crash on cold start",
        Description = "Reproduces on Android 14.",
        Key = "MOB-2",
        ProjectId = projectId,
        StatusId = statusBacklogId,
        Type = IssueType.Bug,
        Priority = IssuePriority.Critical,
        DueDate = (DateTimeOffset?)new DateTimeOffset(2026, 2, 20, 0, 0, 0, TimeSpan.Zero), // 👈 явное приведение
        CreatedDate = DateTimeOffset.UtcNow
    },
    new
    {
        Id = issue3Id,
        Title = "Push notification copy",
        Description = "Finalize strings for v1.",
        Key = "MOB-3",
        ProjectId = projectId,
        StatusId = statusDoneId,
        Type = IssueType.Story,
        Priority = IssuePriority.Low,
        DueDate = (DateTimeOffset?)null, // 👈 уже nullable
        CreatedDate = DateTimeOffset.UtcNow
    }
};

        // IssueTags
        IssueTags = new[]
        {
        new { IssueId = issue1Id, TagId = tagFrontendId },
        new { IssueId = issue1Id, TagId = tagBackendId },
        new { IssueId = issue2Id, TagId = tagBugId },
        new { IssueId = issue2Id, TagId = tagBackendId },
        new { IssueId = issue3Id, TagId = tagFrontendId }
        };

        // Attachments
        Attachments = new[]
        {
        new
        {
            Id = Guid.NewGuid(),
            IssueId = issue1Id,
            UploadedByUserId = aliceId,
            FilePath = "attachments/mob-1/wireframe.png",
            UploadedDate = DateTimeOffset.UtcNow
        },
        new
        {
            Id = Guid.NewGuid(),
            IssueId = issue2Id,
            UploadedByUserId = bobId,
            FilePath = "attachments/mob-2/logcat.txt",
            UploadedDate = DateTimeOffset.UtcNow
        }
        };
    }
}
