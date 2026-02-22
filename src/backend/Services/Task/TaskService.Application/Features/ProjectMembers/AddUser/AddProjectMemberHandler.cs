using TaskService.Application.Exceptions;
using TaskService.Application.Features.WorkspaceMembers.AddUser;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Commands.Workspaces.Create;

public class AddProjectMemberHandler(TaskServiceDbContext context) : IRequestHandler<AddProjectMemberCommand, AddProjectMemberResult>
{
    public async Task<AddProjectMemberResult> Handle(AddProjectMemberCommand command, CancellationToken cancellationToken = default)
    {
        var existProject = await context.Projects.FindAsync(command.ProjectId, cancellationToken);
        if (existProject is null)
        {
            throw new ArgumentNullException("Проект с таким id не существует",
                 nameof(command.ProjectId));
        }

        var existUser = await context.Users.FindAsync(command.UserId, cancellationToken);
        if (existUser is null)
        {
            throw new ArgumentNullException("Пользователь с таким id не существует",
                 nameof(command.ProjectId));
        }

        if (existProject.ProjectMembers.Any(x => x.UserId == existUser.Id))
        {
            throw new ConflictException("Пользователь уже состоит в рабочей области");
        }

        var projectMember = ProjectMember.Create(command.ProjectId, command.UserId, command.RoleDto, DateTimeOffset.UtcNow);

        await context.ProjectMembers.AddAsync(projectMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new AddProjectMemberResult(ProjectId: existProject.Id,
                                 UserId: existUser.Id,
                                 RoleDto: new RoleDto(projectMember.Role.ToString()));
    }
}
