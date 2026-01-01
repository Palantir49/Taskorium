using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities
{
    public class Workspace : BaseEntities
    {
        protected Workspace() { }

        private Workspace(Guid id, string name, DateTimeOffset createdDate, Guid? ownerId) : base(id, name, createdDate)
        {
            OwnerId = ownerId;
        }

        public Guid? OwnerId { get; private set; }

        public static Workspace Create(string name, Guid? ownerId = null)
        {
            return new Workspace(Guid.CreateVersion7(), name, DateTimeOffset.UtcNow, ownerId);
        }
    }
}
