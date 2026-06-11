using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;

namespace TaskService.Application.Features.Tags.Command;

public record class TagCreateCommand(
    string Name,
    Guid ProjectId) : ICommand<TagResponse>;
