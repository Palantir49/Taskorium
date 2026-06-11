using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;
using TaskService.Contracts.Enum;

namespace TaskService.Application.Validators.WorkspaceMember
{
    public class AddWorkspaceMemberCommandValidator : AbstractValidator<AddWorkspaceMemberCommand>
    {
        public AddWorkspaceMemberCommandValidator()
        {
            RuleFor(x => x.UserId)
              .NotEqual(Guid.Empty)
                .WithMessage("Идентификатор пользователя не может быть пустым");

            RuleFor(x => x.WorkspaceId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Идентификатор рабочей области не может быть пустым");

            RuleFor(x => x.Role)
                .Must(x => Enum.IsDefined(x))
                    .WithMessage("Недопустимая роль");
        }
    }
}
