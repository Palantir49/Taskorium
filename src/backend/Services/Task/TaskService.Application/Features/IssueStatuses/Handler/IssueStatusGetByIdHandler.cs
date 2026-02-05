using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueStatuses.Handler
{
    public class IssueStatusGetByIdHandler(TaskServiceDbContext context) : IRequestHandler<IssueStatusGetByIdQuery, IssueStatusResponse>
    {
        public async Task<IssueStatusResponse> Handle(IssueStatusGetByIdQuery request, CancellationToken cancellationToken = default)
        {
            IssueStatus status = await context.IssueStatus.FindAsync(request.id, cancellationToken) ??
                throw new NullReferenceException($"Статус с id: {request.id} не найден");

            return status.ToResponse();
        }
    }
}
