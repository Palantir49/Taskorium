using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.IssueTypes.Handler
{
    public class IssueTypeDeleteByIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueTypeDeleteByIdCommand, int>
    {
        public async Task<int> Handle(IssueTypeDeleteByIdCommand request, CancellationToken cancellationToken = default)
        {
            IssueType type = await wrapper.IssueType.GetByIdAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");

            await wrapper.IssueType.DeleteAsync(type, cancellationToken);
            return await wrapper.SaveChangesAsync(cancellationToken);
        }
    }
}
