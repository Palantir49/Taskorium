using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueTypes.Handler
{
    public class IssueTypeDeleteByIdHandler(TaskServiceDbContext context) : IRequestHandler<IssueTypeDeleteByIdCommand, int>
    {
        public async Task<int> Handle(IssueTypeDeleteByIdCommand request, CancellationToken cancellationToken = default)
        {
            IssueType type = await context.IssueType.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");

            List<Issue> issue = await context.Issues
                .Where(x=>x.IssueTypeId==type.Id).ToListAsync(cancellationToken);

            if (issue != null && issue.Count > 0)
                throw new NullReferenceException($"Нельзя удалить тип задачи, пока существуют связанные задачи");

            context.IssueType.Remove(type);
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
