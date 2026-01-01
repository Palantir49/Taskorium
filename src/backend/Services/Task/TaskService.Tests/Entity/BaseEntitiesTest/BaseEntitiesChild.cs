using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Tests.Entity.BaseEntitiesTest
{
    internal class BaseEntitiesChild : BaseEntities
    {
        public BaseEntitiesChild(Guid id, string name, DateTimeOffset createdDate) : base(id, name, createdDate) { }
    }
}
