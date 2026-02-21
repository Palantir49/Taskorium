using TaskService.Application.Features.IssueTags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueTags.Handler;

public class IssueTagCreateHandler(TaskServiceDbContext context) : IRequestHandler<IssueTagCreateCommand, IssueTagResponse>
{
    public async Task<IssueTagResponse> Handle(IssueTagCreateCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await context.Projects.FindAsync(request.projectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.projectId} не найден");

        IssueTag tag = IssueTag.Create(
            name: request.name,
            projectId: request.projectId,
            color: request.color);

        await context.IssueTag.AddAsync(tag, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return tag.ToResponse();
    }
}
