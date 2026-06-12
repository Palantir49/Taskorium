using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;

namespace TaskService.Application.Features.Tags.Command;

public record class TagUpdateCommand(
    Guid Id,
    string Name) : ICommand<TagResponse>;
