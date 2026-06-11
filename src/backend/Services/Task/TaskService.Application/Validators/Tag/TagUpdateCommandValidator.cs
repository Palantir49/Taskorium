using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Tags.Command;

namespace TaskService.Application.Validators.Tag
{
    public class TagUpdateCommandValidator : AbstractValidator<TagUpdateCommand>
    {
        public TagUpdateCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id не может быть пустым");
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Имя не может быть пустым")
                .Matches(@"^[a-zA-Zа-яА-ЯёЁ0-9 _\-.]+$")
                    .WithMessage("Имя может содержать только буквы, цифры, пробелы, дефисы, подчёркивания и точки.")
                .MaximumLength(50)
                    .WithMessage("Имя не может привышать 50 символов");
        }
    }
}
