using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Users.Write.UpdateUserEmail;

namespace TaskService.Application.Validators.User
{
    public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
    {
        public UpdateUserEmailCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id пользователя не может быть пустым");
            RuleFor(x => x.Email)
                .EmailAddress()
                    .WithMessage((context) => $"Email '{context.Email}' имеет неверный формат");
        }
    }
}
