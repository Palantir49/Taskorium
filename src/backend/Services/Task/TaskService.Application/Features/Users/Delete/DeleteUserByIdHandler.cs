using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Users.Delete
{
    public class DeleteUserByIdAsync(IRepositoryWrapper wrapper) : IRequestHandler<DeleteUserByIdCommand, DeleteUserByIdResult>
    {
        public async Task<DeleteUserByIdResult> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken = default)
        {
            var user = await wrapper.Users.GetByIdAsync(request.id);
            if (user == null)
            {
                throw new NullReferenceException($"Пользователь с id = {request.id} не найден");
            }
            await wrapper.Users.DeleteAsync(user);
            await wrapper.SaveChangesAsync();
            return new DeleteUserByIdResult(user.Id,
                                            user.KeycloakId,
                                            user.Email.Value,
                                            user.Username.Value);
        }
    }
}
