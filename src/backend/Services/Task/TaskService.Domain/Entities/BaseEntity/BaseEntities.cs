using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TaskService.Domain.ValueObjects;

namespace TaskService.Domain.Entities.BaseEntity
{
    public abstract class BaseEntities
    {
        public Guid Id { get; }

        public BaseEntityName Name { get; protected set; }

        public DateTimeOffset CreatedDate { get; }

        protected BaseEntities()
        {
            Name = null!;
        }

        protected BaseEntities(Guid id, string name, DateTimeOffset createdDate)
        {
            Id = id;
            Name = new BaseEntityName(name);
            CreatedDate = createdDate;
        }


        public virtual void UpdateName(string newName)
        {
            Name = new BaseEntityName(newName);
        }
    }
}
