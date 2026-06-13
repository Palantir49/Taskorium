using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Enum;

namespace TaskService.Application.Validators.Dto
{
    public class IssueAssigneesDtoValidator : AbstractValidator<IssueAssigneesDto>
    {
        public IssueAssigneesDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty)
                    .WithMessage("ID пользователя обязателен");

            // Валидация вложенного ProjectRolesDto
            RuleFor(x => x.ProjectRolesDto)
                .Must(x => Enum.IsDefined(x))
                    .WithMessage("Недопустимая роль");
        }
    }
}
