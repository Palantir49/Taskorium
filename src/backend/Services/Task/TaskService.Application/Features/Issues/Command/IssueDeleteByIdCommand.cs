using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Issues.Command
{
    //FAQ: а что должно возвращаться при удалении?
    public record class IssueDeleteByIdCommand(Guid id) : ICommand<int>;
}
