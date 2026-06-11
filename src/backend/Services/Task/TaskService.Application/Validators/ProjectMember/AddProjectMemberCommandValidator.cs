using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Projects.Write.AddProjectMember;
using TaskService.Contracts.Enum;

namespace TaskService.Application.Validators.ProjectMember
{
    public class AddProjectMemberCommandValidator : AbstractValidator<AddProjectMemberCommand>
    {
        public AddProjectMemberCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id пользователя не может быть пустым");
            RuleFor(x => x.ProjectId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Id проекта не может быть пустым");
            RuleFor(x => x.RoleDto)
                .Must(x => Enum.IsDefined(x))
                    .WithMessage("Недопустимая роль пользователя в проекте");
        }
    }
}
