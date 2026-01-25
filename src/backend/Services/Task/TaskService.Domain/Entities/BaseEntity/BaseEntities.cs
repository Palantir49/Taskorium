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
        //FAQ: Вычитал, что можно добавить аля ISystemClock, который будет хранить в себе дату для упрощения тестов и типа аля это лучше (вот почему лучше то?)
        public DateTimeOffset CreatedDate { get; }

        protected BaseEntities()
        {
            Name = null!;
        }

        protected BaseEntities(Guid id, string name)
        {
            Id = id;
            Name = new BaseEntityName(name);
            CreatedDate = DateTimeOffset.UtcNow;
        }


        public virtual void UpdateName(string newName)
        {
            Name = new BaseEntityName(newName);
        }
    }
}
