using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class Attachment : ISoftDeletable
{
    private Attachment(Guid id, Guid issueId, Guid uploaderId, string storagePath)
    {
        Id = id;
        IssueId = issueId;
        UploaderId = uploaderId;
        StoragePath = storagePath;
    }

    public Guid Id { get; private set; }
    public Guid IssueId { get; private set; }

    //TODO need?
    public Guid UploaderId { get; private set; }
    public string StoragePath { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public static Attachment Create(Guid issueId, Guid uploaderId, string fileName)
    {
        var storagePath =
            $"{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}/{DateTime.UtcNow.Day}/{issueId}/{fileName}";
        return new Attachment(Guid.CreateVersion7(), issueId, uploaderId, storagePath);
    }
}
