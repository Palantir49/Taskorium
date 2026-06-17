using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Services;

namespace TaskService.Application.Features.Attachments.AttachmentDownload;

public class AttachmentDownloadHandler(TaskServiceDbContext context,
    FileStorageService fileStorageService) : IRequestHandler<AttachmentDownloadQuery, AttachmentDto>
{
    public async Task<AttachmentDto> Handle(AttachmentDownloadQuery request, CancellationToken cancellationToken = default)
    {
        Attachment attachment = await context.Attachments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ??
                  throw new KeyNotFoundException($"Приложение с id: {request.Id} не найдено");

        var memory = await fileStorageService.DownloadAsync(attachment.StoragePath);

        return new AttachmentDto()
        {
            Content = new MemoryStream(memory.ToArray()),
            ContentType = attachment.ContentType,
            Name = attachment.FileName,
            ContentLength = attachment.ContentLength
        };
    }
}
