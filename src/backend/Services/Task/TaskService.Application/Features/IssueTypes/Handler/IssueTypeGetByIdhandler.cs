using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueTypes.Handler
{
    public class IssueTypeGetByIdhandler(TaskServiceDbContext context) : IRequestHandler<IssueTypeGetByIdQuery, IssueTypeResponse>
    {
        public async Task<IssueTypeResponse> Handle(IssueTypeGetByIdQuery request, CancellationToken cancellationToken = default)
        {
            IssueType type = await context.IssueType.FindAsync(request.id, cancellationToken) ??
                throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");
            return type.ToResponse();
        }
    }
}
