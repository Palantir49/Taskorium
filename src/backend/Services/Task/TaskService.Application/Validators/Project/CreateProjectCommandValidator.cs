using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Projects.Write.CreateProject;

namespace TaskService.Application.Validators.Project
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                   .WithMessage("Имя проекта не может быть пустым")
                .Matches(@"^[a-zA-Zа-яА-Я0-9_ ]+$")
                    .WithMessage("Только буквы, цифры, пробел и подчёркивание")
                .MaximumLength(225)
                    .WithMessage("Наименование не может быть длиннее 225 символов");
            RuleFor(x => x.Description)
                .MaximumLength(2000)
                    .WithMessage("Описание не может быть длиннее 2000 символов");
            RuleFor(x => x.Abbreviation)
                .NotEmpty()
                   .WithMessage("Аббревиатура не может быть пустым")
                .MaximumLength(5)
                    .WithMessage("Аббревиатура не может быть длиннее 5 символов");
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id пользователя создателя не может быть пустым");
            RuleFor(x => x.WorkspaceId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id рабочей области не может быть пустым");

        }
    }
}
