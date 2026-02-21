using TaskService.Application.Features.IssueTags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueTags.Handler
{
    public class IssueTagGetByIdhandler(TaskServiceDbContext context) : IRequestHandler<IssueTagGetByIdQuery, IssueTagResponse>
    {
        public async Task<IssueTagResponse> Handle(IssueTagGetByIdQuery request, CancellationToken cancellationToken = default)
        {
            IssueTag tag = await context.IssueTag.FindAsync(request.id, cancellationToken) ??
                throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");
            return tag.ToResponse();
        }
    }
}
