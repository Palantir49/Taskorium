using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
﻿using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Services;

namespace TaskService.Application.Features.Attachments.AttachmentDelete;

public class AttachmentDeleteHandler(TaskServiceDbContext context,
    FileStorageService fileStorageService,
    HybridCache cache) : IRequestHandler<AttachmentDeleteQuery, bool>
{
    public async Task<bool> Handle(AttachmentDeleteQuery request, CancellationToken cancellationToken = default)
    {
        Attachment attachment = await context.Attachments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Вложение не найдено"); ;
        var issueId = attachment.IssueId;
        var projectId = await context.Issues
                            .Where(x => x.Id == issueId)
                            .Select(x => (Guid?)x.ProjectId)
                            .FirstOrDefaultAsync(cancellationToken)
                        ?? throw new KeyNotFoundException($"Задача для вложения с id: {request.Id} не найдена");

        try
        {
            await fileStorageService.DeleteAsync(attachment.StoragePath, cancellationToken);
        }
        catch
        {
            //TODO: logger
        }

        attachment.IsDeleted = true;
        attachment.DeletedAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync($"issue_id_{issueId}", cancellationToken);
        await cache.RemoveAsync($"issue_id_v2_{issueId}", cancellationToken);
        await cache.RemoveAsync($"issues_by_project_id_{projectId}", cancellationToken);
        await cache.RemoveAsync($"issues_by_project_id_v2_{projectId}", cancellationToken);

        return true;
    }
}
