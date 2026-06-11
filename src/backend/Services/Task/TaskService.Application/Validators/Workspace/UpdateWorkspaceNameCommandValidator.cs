using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Workspaces.Write.UpdateWorkspaceName;

namespace TaskService.Application.Validators.Workspace
{
    public class UpdateWorkspaceNameCommandValidator : AbstractValidator<UpdateWorkspaceNameCommand>
    {
        public UpdateWorkspaceNameCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id рабочей области не может быть пустым");
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Имя рабочей области не может быть пустым")
                .Matches(@"^[a-zA-Zа-яА-ЯёЁ0-9 _\-.]+$")
                    .WithMessage("Имя может содержать только буквы, цифры, пробелы, дефисы, подчёркивания и точки.")
                .MaximumLength(50)
                    .WithMessage("Имя рабочей области не может привышать 50 символов");
        }
    }
}
