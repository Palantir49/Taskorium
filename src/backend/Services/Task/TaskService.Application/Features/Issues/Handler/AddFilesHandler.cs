using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Interfaces;
using TaskService.Application.Mediator;
using TaskService.Contracts.Attachment;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Services;

namespace TaskService.Application.Features.Issues.Handler
{
    public class AddFilesHandler(
        TaskServiceDbContext context,
        FileStorageService fileStorageService,
        ICurrentUserContext currentUser)
        : IRequestHandler<AddFilesCommand, IEnumerable<AttachmentResponce>>
    {
        public async Task<IEnumerable<AttachmentResponce>> Handle(AddFilesCommand request, CancellationToken cancellationToken = default)
        {
            if (request.Attachments == null || request.Attachments.Count == 0)
            {
                return new List<AttachmentResponce>();
            }
            List<Attachment> attachments = new(request.Attachments.Count);

                try
                {
                    foreach (var attach in request.Attachments)
                    {
                        var attachment = Attachment.Create(
                            request.IssueId,
                            currentUser.User.Id,
                            attach.Name,
                            attach.ContentType,
                            attach.ContentLength);

                        //сброс позиции чтения файла
                        if (attach.Content.CanSeek)
                        {
                            attach.Content.Position = 0;
                        }

                        await fileStorageService.UploadAsync(
                            attachment.StoragePath,
                            attach.ContentType,
                            attach.Content,
                            cancellationToken);

                        context.Attachments.Add(attachment);
                        attachments.Add(attachment);
                    }
                    await context.SaveChangesAsync(cancellationToken);
                    return attachments.Select(x => new AttachmentResponce(Id: x.Id, Name: x.FileName));
                }
                catch
                {
                    if (attachments.Count > 0)
                    {
                        var tasks = attachments.Select(async delete =>
                        {
                            try
                            {
                                await fileStorageService.DeleteAsync(delete.StoragePath, cancellationToken);
                            }
                            catch
                            {
                                //TODO: logger
                            }
                        });
                        await Task.WhenAll(tasks);
                    }

                    throw;
                }
        }
    }
}
