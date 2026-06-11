using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Exceptions;
using TaskService.Application.Features.Workspaces.Write.CreateWorkspace;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Users.Write.CreateUser;

public class CreateUserHandler(TaskServiceDbContext context, IValidator<CreateUserCommand> validator)
    : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        //проверим существует ли пользователь
        //var user = await context.Users.FirstOrDefaultAsync(element => element.KeycloakId == command.KeycloakId,
        //    cancellationToken);
        var user = await context.Users.FirstOrDefaultAsync(element => element.KeycloakId == command.KeycloakId,
            cancellationToken);
        if (user is not null)
        {
            throw new ConflictException("Пользователь уже существует");
        }

        var newUser = User.Create(command.KeycloakId, command.Username, command.Email, command.Name);

        await context.Users.AddAsync(newUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return new CreateUserResult(newUser.Id, newUser.Email.ToString());
        // return new UserResponse(id: user.Id, keycloakId: user.KeycloakId, email: user.Email.ToString(), username: user.Username.ToString(), createdAt: user.CreatedDate);
    }
}
