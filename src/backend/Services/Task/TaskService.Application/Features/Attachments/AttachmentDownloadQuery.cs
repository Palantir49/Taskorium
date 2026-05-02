using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Attachments
{
    public record AttachmentDownloadQuery(Guid Id): IQuery<AttachmentDto>;
}
