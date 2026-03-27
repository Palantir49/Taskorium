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

    }
}
