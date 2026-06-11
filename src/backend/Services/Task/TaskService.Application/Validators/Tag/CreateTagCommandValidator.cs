using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Tags.Command;

namespace TaskService.Application.Validators.Tag
{
    public class CreateTagCommandValidator : AbstractValidator<TagCreateCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Имя не может быть пустым")
                .Matches(@"^[a-zA-Zа-яА-ЯёЁ0-9 _\-.]+$")
                    .WithMessage("Имя может содержать только буквы, цифры, пробелы, дефисы, подчёркивания и точки.")
                .MaximumLength(50)
                    .WithMessage("Имя не может привышать 50 символов");
            RuleFor(x => x.ProjectId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id проекта не может быть пустым ");
        }
    }
}
