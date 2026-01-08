using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities
{
    public class WorkspaceMembers : BaseEntities
    {
        public Guid WorkspaceId { get; private set; }
    }
}
