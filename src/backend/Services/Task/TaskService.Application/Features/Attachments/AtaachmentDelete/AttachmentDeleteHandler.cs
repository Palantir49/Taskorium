using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Services;

namespace TaskService.Application.Features.Attachments
{
    internal class AttachmentDeleteHandler(TaskServiceDbContext context,
        FileStorageService fileStorageService) : IRequestHandler<AttachmentDeleteQuery, bool>
    {
        public async Task<bool> Handle(AttachmentDeleteQuery request, CancellationToken cancellationToken = default)
        {
            Attachment attachment = await context.Attachments.FindAsync([request.Id], cancellationToken)
                ?? throw new KeyNotFoundException($"Вложение не найдено"); ;
            try
            {
                await fileStorageService.DeleteAsync(attachment.StoragePath, cancellationToken);
            }
            catch
            {
                //TODO: logger
            }
            return true;
        }
    }
}
