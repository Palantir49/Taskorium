using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities
{
    public class Issue : BaseEntities
    {
        public Guid ProjectId { get; }
        public Guid IssueTagId { get; private set; }
        public Guid IssueStatusId { get; private set; }
        public string? Description { get; private set; }
        public IssueType IssueType { get; set; }
        public DateTimeOffset? ResolvedDate { get; private set; }
        public DateTimeOffset? UpdatedDate { get; private set; }
        public DateTimeOffset? DueDate { get; private set; }

        //TODO: Добавление свойств:
        //ключ 
        //FAQ: как его создавать? возможно в хенделере запрашить проект, брать его короткое имя и количество задач в нем. "PROJ-123"
        //дата назначения - возможно нужно в таблицу 
        //дата взятия в работу - может добавить таблицу истории? статусов "в работе" может быть несколько
        //FAQ: а какой жизненный цикл у этого свойства? ведь может быть ситуация случайного перевода в рабочий статус и обратная ситуация, когда случайно перенесли в рабочую

        protected Issue() { }

        private Issue(Guid id, string name, string? description, Guid projectId, Guid taskTagId, Guid taskStatusId, int numberIssueType, DateTimeOffset? dueDate) : base(id, name)
        {
            ProjectId = projectId;
            IssueTagId = taskTagId;
            IssueStatusId = taskStatusId;
            Description = description?.Trim();
            DueDate = dueDate;
            IssueType = (IssueType)numberIssueType;
        }

        public static Issue Create(string name, string? description, Guid projectId, Guid taskTagId, Guid taskStatusId, int numberIssueType, DateTimeOffset? dueDate)
        {
            return new Issue(
                id: Guid.CreateVersion7(), 
                name: name, 
                description: description, 
                projectId: projectId, 
                taskTagId: taskTagId, 
                taskStatusId: taskStatusId,
                numberIssueType: numberIssueType,
                dueDate: dueDate);
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

        public void UpdateStatus(IssueStatus status)
        {
            IssueStatusId = status.Id;
            UpdatedDate = DateTimeOffset.UtcNow;

            if (status.Type == IssueStatusType.Success || status.Type == IssueStatusType.Rejected)
                ResolvedDate = DateTimeOffset.UtcNow;
            else
                ResolvedDate = null;
        }

        public void UpdateTag(IssueTag issueTag)
        {
            IssueTagId = issueTag.Id;
            UpdatedDate = DateTimeOffset.UtcNow;
        }

        public void UpdateType(int numberType)
        {
            IssueType = (IssueType)numberType;
            UpdatedDate = DateTimeOffset.UtcNow;
        }
    }
}
