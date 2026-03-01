using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Tags.Command
{
    public record class TagDeleteByIdCommand(Guid id) : ICommand<int>;
}
