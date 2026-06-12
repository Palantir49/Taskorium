using FluentValidation;
using TaskService.Application.Mediator;
using TaskService.Domain.ValueObjects;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Users.Write.UpdateUserEmail
{
    public class UpdateUserEmailHandler(TaskServiceDbContext context, IValidator<UpdateUserEmailCommand> validator)
        : IRequestHandler<UpdateUserEmailCommand, UpdateUserEmailResult>
    {
        public async Task<UpdateUserEmailResult> Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken = default)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var user = await context.Users.FindAsync(request.Id);
            if (user == null)
            {
                throw new NullReferenceException($"Пользователь с id = {request.Id} не найден");
            }
            user.UpdateEmail(new EmailAdress(request.Email));
            await context.SaveChangesAsync();
            return new UpdateUserEmailResult(user.Id,
                                            user.KeycloakId,
                                            user.Email.Value,
                                            user.Username.Value);
        }
    }
}
