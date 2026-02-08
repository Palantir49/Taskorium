using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities
{
    public class WorkspaceMember
    {
        public Guid UserId { get; private set; }
        public Guid WorkspaceId { get; private set; }
    }
}
