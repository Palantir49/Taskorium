using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities
{
    internal class Workspace : BaseEntityTask
    {
        private Workspace(Guid id, string name, DateTimeOffset createdDate) : base(id, name, createdDate) { }

        public static Workspace Create(string name)
        {
            return new Workspace(Guid.CreateVersion7(), name, DateTimeOffset.UtcNow);
        }
    }
}
