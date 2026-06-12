using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.ProjectMembers.Write.UpdateProjectMemberRole;

namespace TaskService.Application.Validators.ProjectMember
{
    public class UpdateProjectMemberRoleCommandValidator : AbstractValidator<UpdateProjectMemberRoleCommand>
    {
        public UpdateProjectMemberRoleCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty)
                    .WithMessage("ID пользователя обязателен");

            RuleFor(x => x.NewRole)
                .Must(x => Enum.IsDefined(x))
                    .WithMessage("Недопустимая роль участника проекта");
        }
    }
}
