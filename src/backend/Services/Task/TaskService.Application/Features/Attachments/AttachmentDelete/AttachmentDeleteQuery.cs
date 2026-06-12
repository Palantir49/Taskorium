using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Attachments.AttachmentDelete;

public record AttachmentDeleteQuery(Guid Id) : ICommand<bool>;
