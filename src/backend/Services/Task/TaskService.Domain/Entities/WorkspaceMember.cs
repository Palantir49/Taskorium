using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities
{
    public class WorkspaceMember
    {
        public Guid UserId { get; private set; }
        public Guid WorkspaceId { get; private set; }
        public Roles Role { get; private set; }
        public DateTimeOffset JoinedAt { get; private set; }
        public WorkspaceMember()
        {

        }
        private WorkspaceMember(Guid workspaceId, Guid userId, Roles role, DateTimeOffset joinedAt)
        {
            UserId = userId;
            WorkspaceId = workspaceId;
            Role = role;
            JoinedAt = joinedAt;
        }
        public static WorkspaceMember Create(Guid workspaceId, Guid userId, Roles role, DateTimeOffset joinedAt)
        {
            return new WorkspaceMember(workspaceId, userId, role, joinedAt);
        }
        public static WorkspaceMember Create(Guid workspaceId, Guid userId, Roles role) => Create(workspaceId, userId, role, DateTimeOffset.UtcNow);
    }
}
