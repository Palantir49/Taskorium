using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Tags.Handler
{
    public class TagDeleteByIdHandler(TaskServiceDbContext context) : IRequestHandler<TagDeleteByIdCommand, int>
    {
        public async Task<int> Handle(TagDeleteByIdCommand request, CancellationToken cancellationToken = default)
        {
            Tag tag = await context.Tags.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");

            //List<Issue> issue = await context.Issues
            //    .Where(x => x.IssueTagId == tag.Id).ToListAsync(cancellationToken);

            //if (issue != null && issue.Count > 0)
            //    throw new NullReferenceException($"Нельзя удалить тип задачи, пока существуют связанные задачи");

            //context.IssueTag.Remove(tag);
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
