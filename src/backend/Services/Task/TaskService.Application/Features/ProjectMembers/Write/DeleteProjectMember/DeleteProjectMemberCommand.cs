using TaskService.Application.Mediator;

namespace TaskService.Application.Features.ProjectMembers.Write.DeleteProjectMember
{
    public record DeleteProjectMemberCommand(Guid ProjectId, Guid UserId) : ICommand<int>;
}
