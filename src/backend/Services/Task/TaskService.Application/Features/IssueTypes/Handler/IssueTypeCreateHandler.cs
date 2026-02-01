using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.IssueTypes.Handler;

public class IssueTypeCreateHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueTypeCreateCommand, IssueTypeResponse>
{
    public async Task<IssueTypeResponse> Handle(IssueTypeCreateCommand request, CancellationToken cancellationToken = default)
    {
        IssueType type = IssueType.Create(
            name: request.name,
            projectId: request.projectId,
            color: request.color);

        await wrapper.IssueType.AddAsync(type, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);
        return type.ToResponse();
    }
}
