using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities
{
    public class Issue : BaseEntities
    {
        public Guid ProjectId { get; }
        public Guid TaskTypeId { get; private set; }
        public Guid TaskStatusId { get; private set; }
        public string? Description { get; private set; }
        public User? User { get; private set; }
        public DateTimeOffset? ResolvedDate { get; private set; }
        public DateTimeOffset? UpdatedDate { get; private set; }
        public DateTimeOffset? DueDate { get; private set; }

        //TODO: Добавление свойств:
        //ключ 
        //FAQ: как его создавать? возможно в хенделере запрашить проект, брать его короткое имя и количество задач в нем. "PROJ-123"
        //дата назначения
        //дата взятия в работу 
        //FAQ: а какой жизненный цикл у этого свойства? ведь может быть ситуация случайного перевода в рабочий статус и обратная ситуация, когда случайно перенесли в рабочую

        protected Issue() { }

        private Issue(Guid id, string name, string? description, Guid projectId, Guid taskTypeId, Guid taskStatusId, DateTimeOffset? updatedDate, DateTimeOffset? dueDate,
        DateTimeOffset? resolvedDate) : base(id, name)
        {
            ProjectId = projectId;
            TaskTypeId = taskTypeId;
            TaskStatusId = taskStatusId;
            Description = description?.Trim();
            UpdatedDate = updatedDate;
            DueDate = dueDate;
            ResolvedDate = resolvedDate;
        }

        public static Issue Create(string name, string? description, Guid projectId, Guid taskTypeId, Guid taskStatusId, DateTimeOffset? dueDate)
        {
            return new Issue(Guid.CreateVersion7(), name, description, projectId, taskTypeId, taskStatusId,
                null, null, dueDate);
        }

        public override void UpdateName(string newName)
        {
            base.UpdateName(newName);
            UpdatedDate = DateTimeOffset.UtcNow;
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription;
            UpdatedDate = DateTimeOffset.UtcNow;
        }

        public void UpdateDueDate(DateTimeOffset? newDueDate)
        {
            DueDate = newDueDate;
            UpdatedDate = DateTimeOffset.UtcNow;
        }

        //DESIGN: статус должен иметь значение, которое будет указывать, является ли он завершающим или начальным для изменения дат начала и завершения задачи.
        public void ChangeStatus(Guid newTaskStatusId, bool resolved = false)
        {
            TaskStatusId = newTaskStatusId;
            UpdatedDate = DateTimeOffset.UtcNow;
            ResolvedDate = resolved ? DateTimeOffset.UtcNow : null;
        }

        public void UpdateType(Guid newTaskTypeId)
        {
            TaskTypeId = newTaskTypeId;
            UpdatedDate = DateTimeOffset.UtcNow;
        }
    }
}
