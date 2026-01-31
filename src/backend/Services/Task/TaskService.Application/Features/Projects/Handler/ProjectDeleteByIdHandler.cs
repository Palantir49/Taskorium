using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Projects.Handler;

public class ProjectDeleteByIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<ProjectDeleteByIdCommand, int>
{
    public async Task<int> Handle(ProjectDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await wrapper.Projects.GetByIdAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.id} не найдена");

        await wrapper.Projects.DeleteAsync(project, cancellationToken);
        return await wrapper.SaveChangesAsync(cancellationToken);
    }
}
