using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Tests.Entity.BaseEntitiesTest
{
    internal class BaseEntitiesTestChild : BaseEntities
    {
        public BaseEntitiesTestChild(Guid id, string name) : base(id, name) { }
    }
}
