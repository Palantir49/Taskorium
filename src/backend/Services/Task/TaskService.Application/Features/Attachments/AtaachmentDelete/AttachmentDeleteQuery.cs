using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Attachments;

public record AttachmentDeleteQuery(Guid Id) : ICommand<bool>;
