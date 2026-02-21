using TaskService.Application.Mediator;

namespace TaskService.Application.Features.IssueTags.Command
{
    public record class IssueTagDeleteByIdCommand(Guid id) : ICommand<int>;
}
