using FluentValidation;
using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskService.Application.Features.Tags.Handler;

public class TagCreateHandler(TaskServiceDbContext context, IValidator<TagCreateCommand> validator) : IRequestHandler<TagCreateCommand, TagResponse>
{
    public async Task<TagResponse> Handle(TagCreateCommand request, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Project project = await context.Projects.FindAsync(request.ProjectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.ProjectId} не найден");

        Tag tag = Tag.Create(
            name: request.Name,
            projectId: request.ProjectId);

        await context.Tags.AddAsync(tag, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return tag.ToResponse();
    }
}
