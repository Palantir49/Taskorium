using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Projects.Write.UpdateProject;

namespace TaskService.Application.Validators.Project
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.id)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id проекта не может быть пустым");
            RuleFor(x => x.Name)
               .NotEmpty()
                   .WithMessage("Имя проекта не может быть пустым")
               .Matches(@"^[a-zA-Zа-яА-Я0-9_]+$")
                   .WithMessage("Только буквы, цифры и подчеркивание")
                .MaximumLength(225)
                    .WithMessage("Наименование не может быть длиннее 225 символов");
            RuleFor(x => x.Description)
                .NotEmpty()
                   .WithMessage("Описание проекта не может быть пустым")
               .MaximumLength(2000)
                   .WithMessage("Описание не может быть длиннее 2000 символов");
        }
    }
}
