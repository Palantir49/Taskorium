using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Tags.Handler;

public class TagUpdateHandler(TaskServiceDbContext context, IValidator<TagUpdateCommand> validator) : IRequestHandler<TagUpdateCommand, TagResponse>
{
    public async Task<TagResponse> Handle(TagUpdateCommand request, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Tag tag = await context.Tags.FindAsync(request.Id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.Id} не найден");

        tag.UpdateName(request.Name);

        context.Tags.Update(tag);
        await context.SaveChangesAsync(cancellationToken);
        return tag.ToResponse();
    }
}
