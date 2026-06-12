using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Attachments.AttachmentDownload;

public record AttachmentDownloadQuery(Guid Id) : ICommand<AttachmentDto>;
