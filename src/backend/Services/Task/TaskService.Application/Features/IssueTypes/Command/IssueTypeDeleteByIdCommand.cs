using TaskService.Application.Mediator;

namespace TaskService.Application.Features.IssueTypes.Command
{
    public record class IssueTypeDeleteByIdCommand(Guid id) : ICommand<int>;
}
