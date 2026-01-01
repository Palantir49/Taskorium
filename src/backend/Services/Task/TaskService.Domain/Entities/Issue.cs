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
        public Guid? ReporterId { get; private set; }
        //TODO: возможно стоит добавить дату начала, чтобы условно выводить промежуток, котогда она в работе
        //+ когда назначена и когда в работу взяли
        public DateTimeOffset? ResolvedDate { get; private set; }
        public DateTimeOffset? UpdatedDate { get; private set; }
        public DateTimeOffset? DueDate { get; private set; }

        //TODO: уточтить за ключ
        //сокращение проекта + номер задачи по порядку
        //         //public varchar(20) key "PROJ-123"

        protected Issue() { }

        private Issue(Guid id, string name, string? description, Guid projectId, Guid taskTypeId, Guid taskStatusId,
        Guid? reporterId, DateTimeOffset createdDate, DateTimeOffset? updatedDate, DateTimeOffset? dueDate,
        DateTimeOffset? resolvedDate) : base(id, name, createdDate)
        {
            ProjectId = projectId;
            TaskTypeId = taskTypeId;
            TaskStatusId = taskStatusId;
            Description = description?.Trim();
            ReporterId = reporterId;
            UpdatedDate = updatedDate;
            DueDate = dueDate;
            ResolvedDate = resolvedDate;
        }

        public static Issue Create(string name, string? description, Guid projectId, Guid taskTypeId, Guid taskStatusId,
        Guid? reporterId, DateTimeOffset? dueDate)
        {
            return new Issue(Guid.CreateVersion7(), name, description, projectId, taskTypeId, taskStatusId, reporterId,
            DateTimeOffset.UtcNow, null, null, dueDate);
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

        //TODO: а как изменять ResolvedDate? по сути оно должно ставится с соответствующим статусом.
        //уточнить этот момент, а пока bool
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
