using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TaskService.Domain.Entities;

public class Attachment
{
    public Guid Id { get; private set; }
    public Guid IssueId { get; private set; }
    public Guid UploaderId { get; private set; }
    public string StoragePath { get; private set; } = null!;
    private Attachment(Guid id, Guid issueId, Guid uploaderId, string storagePath)
    {
        Id = id;
        IssueId = issueId;
        UploaderId = uploaderId;
        StoragePath = storagePath;
    }
    public static Attachment Create(Guid issueId, Guid uploaderId, string storagePath)
    {
        return new Attachment(Guid.CreateVersion7(), issueId, uploaderId, storagePath);
    }

}
