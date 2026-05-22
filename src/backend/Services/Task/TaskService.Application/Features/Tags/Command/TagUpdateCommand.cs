using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;

namespace TaskService.Application.Features.Tags.Command;

public record class TagUpdateCommand(
    Guid id,
    string name) : ICommand<TagResponse>;
