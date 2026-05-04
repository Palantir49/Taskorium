using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities
{
    public class WorkspaceMember : ISoftDeletable
    {
        public Guid UserId { get; private set; }
        public Guid WorkspaceId { get; private set; }
        public WorkspaceRoles Role { get; private set; }
        public DateTimeOffset JoinedAt { get; private set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public Workspace Workspace { get; private set; } = null!;
        public User User { get; private set; } = null!;
        public WorkspaceMember()
        {

        }
        private WorkspaceMember(Guid workspaceId, Guid userId, WorkspaceRoles role, DateTimeOffset joinedAt)
        {
            UserId = userId;
            WorkspaceId = workspaceId;
            Role = role;
            JoinedAt = joinedAt;
        }
        public static WorkspaceMember Create(Guid workspaceId, Guid userId, WorkspaceRoles role, DateTimeOffset joinedAt)
        {
            return new WorkspaceMember(workspaceId, userId, role, joinedAt);
        }
        public static WorkspaceMember Create(Guid workspaceId, Guid userId, WorkspaceRoles role) => Create(workspaceId, userId, role, DateTimeOffset.UtcNow);
    }
}
