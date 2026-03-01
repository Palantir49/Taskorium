using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;
using TaskService.Domain.ValueObjects;

namespace TaskService.Domain.Entities
{
    public class Issue : BaseEntities
    {
        public Guid ProjectId { get; }
        public Guid IssueStatusId { get; private set; }
        public string? Description { get; private set; }
        public IssueKey Key { get; private set; } = null!;
        public IssueType IssueType { get; private set; }
        public IssuePriority IssuePriority { get; private set; }
        public DateTimeOffset? StartDate { get; private set; }
        public DateTimeOffset? ResolvedDate { get; private set; }
        public DateTimeOffset? UpdatedDate { get; private set; }
        public DateTimeOffset? DueDate { get; private set; }

        public List<Tag> Tags { get; private set; } = new();

        protected Issue() { }

        private Issue(Guid id, string name, string? description, string key, Guid projectId, Guid taskStatusId, int numberIssueType, int numberIssuePriority, DateTimeOffset? dueDate) : base(id, name)
        {
            ProjectId = projectId;
            Key = new IssueKey(key);
            IssueStatusId = taskStatusId;
            Description = description?.Trim();
            DueDate = dueDate;
            IssueType = (IssueType)numberIssueType;
            IssuePriority = (IssuePriority)numberIssuePriority;
        }

        public static Issue Create(string name, string? description, string key, Guid projectId, Guid taskStatusId, int numberIssueType, int numberIssuePriority, DateTimeOffset? dueDate)
        {
            return new Issue(
                id: Guid.CreateVersion7(),
                name: name,
                description: description,
                key: key,
                projectId: projectId,
                taskStatusId: taskStatusId,
                numberIssueType: numberIssueType,
                numberIssuePriority: numberIssuePriority,
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

            if (status.Type == IssueStatusType.Process && StartDate == default(DateTimeOffset))
                StartDate = DateTimeOffset.UtcNow;

            if (status.Type == IssueStatusType.Success || status.Type == IssueStatusType.Rejected)
                ResolvedDate = DateTimeOffset.UtcNow;
            else
                ResolvedDate = null;
        }

        public void UpdateType(int numberType)
        {
            IssueType = (IssueType)numberType;
            UpdatedDate = DateTimeOffset.UtcNow;
        }
    }
}
