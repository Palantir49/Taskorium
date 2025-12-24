using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.Entities.BaseEntity
{
    public abstract class BaseEntityTask
    {
        public Guid Id { get; }

        public string Name { get; protected set; }

        public DateTimeOffset CreatedDate { get; }
        protected BaseEntityTask(Guid id, string name, DateTimeOffset createdDate)
        {
            IsValidName(name);
            Id = id;
            Name = name;
            CreatedDate = createdDate;
        }

        private void IsValidName(string? value)
        {
            //TODO: уточнить за ошибки на русском
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Наименование задачи не может быть пустым",
                nameof(value));
        }

        public virtual void UpdateName(string newName)
        {
            Name = newName.Trim();
            IsValidName(Name);
        }
    }
}
