using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;

namespace TaskService.Application.Validators.Workspace
{
    internal class AddWorkspaceMemberCommandValidator:AbstractValidator<AddWorkspaceMemberCommand>
    {
        public AddWorkspaceMemberCommandValidator()
        {
              RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Идентификатор пользователя не может быть пустым");

            RuleFor(x => x.WorkspaceId)
                .NotEmpty()
                .WithMessage("Идентификатор рабочей области не может быть пустым");
            RuleFor(x => x.Role)
               .NotEmpty()
               .WithMessage("Роль не может быть пустой");
        }
    }
}
