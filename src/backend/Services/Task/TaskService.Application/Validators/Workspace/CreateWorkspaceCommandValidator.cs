using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Workspaces.Write.CreateWorkspace;

namespace TaskService.Application.Validators.Workspace
{
    public class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
    {
        public CreateWorkspaceCommandValidator()
        {
            RuleFor(x => x.OwnerId)
                .NotEmpty()
                .WithMessage("Ошибка создания рабочей области. Идентификатор создателя не может быть пустым");

            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[a-zA-Zа-яА-ЯёЁ0-9 _\-.]+$")
                .WithMessage("Имя может содержать только буквы, цифры, пробелы, дефисы, подчёркивания и точки.")
                .MaximumLength(50)
                .WithMessage("Имя рабочей области не может привышать 50 символов");


        }
    }
}
