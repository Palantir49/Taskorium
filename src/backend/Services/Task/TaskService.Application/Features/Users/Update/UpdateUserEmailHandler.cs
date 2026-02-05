using TaskService.Application.Mediator;
using TaskService.Domain.Repositories;
using TaskService.Domain.ValueObjects;

namespace TaskService.Application.Features.Users.Update
{
    public class UpdateUserEmailHandler(IRepositoryWrapper wrapper) : IRequestHandler<UpdateUserEmailCommand, UpdateUserEmailResult>
    {
        public async Task<UpdateUserEmailResult> Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken = default)
        {
            var user = await wrapper.Users.GetByIdAsync(request.id);
            if (user == null)
            {
                throw new NullReferenceException($"Пользователь с id = {request.id} не найден");
            }
            user.UpdateEmail(new EmailAdress(request.email));
            await wrapper.SaveChangesAsync();
            return new UpdateUserEmailResult(user.Id,
                                            user.KeycloakId,
                                            user.Email.Value,
                                            user.Username.Value);
        }
    }
}
