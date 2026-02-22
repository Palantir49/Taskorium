using TaskService.Application.Exceptions;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Features.WorkspaceMembers.AddUser;

public class AddWorkspaceMemberHandler(TaskServiceDbContext context) : IRequestHandler<AddWorkspaceMemberCommand, AddWorkspaceMemberResult>
{
    public async Task<AddWorkspaceMemberResult> Handle(AddWorkspaceMemberCommand command, CancellationToken cancellationToken = default)
    {
        var existWorkspace = await context.Workspaces.FindAsync(command.workspaceId, cancellationToken);
        if (existWorkspace is null)
        {
            throw new ArgumentNullException("Рабочей области с таким id не существует",
                 nameof(command.workspaceId));
        }

        var existUser = await context.Users.FindAsync(command.userId, cancellationToken);
        if (existUser is null)
        {
            throw new ArgumentNullException("Пользователь с таким id не существует",
                 nameof(command.workspaceId));
        }

        if (existWorkspace.WorkspaceMembers.Any(x => x.UserId == existUser.Id))
        {
            throw new ConflictException("Пользователь уже состоит в рабочей области");
        }

        var workspaceMember = WorkspaceMember.Create(command.workspaceId, command.userId, command.role, DateTimeOffset.UtcNow);

        await context.WorkspaceMembers.AddAsync(workspaceMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new AddWorkspaceMemberResult(WorkspaceId: existWorkspace.Id,
                                 UserId: existUser.Id,
                                 RoleDto: new RoleDto(workspaceMember.Role.ToString()));
    }
}
