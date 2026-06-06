using TaskService.Application.Mediator;

namespace TaskService.Application.Features.WorkspaceMembers.Write.DeleteWorkspaceMember
{
    public record DeleteWorkspaceMemberCommand(Guid WorkspaceId, Guid UserId) : ICommand<int>;
}
