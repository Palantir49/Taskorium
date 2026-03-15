using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;

namespace TaskService.Application.Features.Tags.Command;

public record class TagGetByProjectIdQuery(Guid id) : IQuery<IEnumerable<TagResponse>>;
