using TaskService.Application.Features.Users.Write.Command;
using TaskService.Application.Features.Users.Write.Result;
using TaskService.Application.Mediator;
using TaskService.Domain.ValueObjects;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Users.Write.Handler
{
    public class UpdateUserEmailHandler(TaskServiceDbContext context) : IRequestHandler<UpdateUserEmailCommand, UpdateUserEmailResult>
    {
        public async Task<UpdateUserEmailResult> Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken = default)
        {
            var user = await context.Users.FindAsync(request.id);
            if (user == null)
            {
                throw new NullReferenceException($"Пользователь с id = {request.id} не найден");
            }
            user.UpdateEmail(new EmailAdress(request.email));
            await context.SaveChangesAsync();
            return new UpdateUserEmailResult(user.Id,
                                            user.KeycloakId,
                                            user.Email.Value,
                                            user.Username.Value);
        }
    }
}
