using FluentValidation;
using TaskService.Application.Features.Users.Write.CreateUser;

namespace TaskService.Application.Validators.User;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(context => $"Email '{context.Email}' имеет неверный формат");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Имя пользователя обязательно")
            .MinimumLength(3)
            .WithMessage("Минимальная длина - 3 символа")
            .MaximumLength(50)
            .WithMessage("Максимальная длина - 50 символов")
            .Matches(@"^[a-zA-Zа-яА-Я0-9_\. ]+$")
            .WithMessage("Только буквы, цифры, пробелы и подчеркивание");

        RuleFor(x => x.Name)
            .Matches(@"^[a-zA-Zа-яА-ЯёЁ\s-]*$")
            .WithMessage("Имя содержит недопустимые символы");
    }
}
