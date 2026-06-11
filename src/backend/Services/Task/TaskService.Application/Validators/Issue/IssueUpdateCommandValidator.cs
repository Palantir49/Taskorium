using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Validators.Dto;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Validators.Issue
{
    public class IssueUpdateCommandValidator : AbstractValidator<IssueUpdateCommand>
    {
        public IssueUpdateCommandValidator()
        {
            RuleFor(x => x.Name)
           .NotEmpty()
               .WithMessage("Название задачи обязательно")
           .MinimumLength(3)
               .WithMessage("Название должно содержать минимум 3 символа")
           .MaximumLength(255)
               .WithMessage("Название не может быть длиннее 255 символов")
           .Matches(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\-_.,!?()]+$")
               .WithMessage("Название содержит недопустимые символы");

           
            // Описание
            RuleFor(x => x.Description)
                .MaximumLength(5000)
                    .WithMessage("Описание не может быть длиннее 5000 символов")
                .When(x => x.Description != null);

            // Дата выполнения
            RuleFor(x => x.DueDate)
                .Must(date => !date.HasValue || date.Value > DateTimeOffset.UtcNow)
                    .WithMessage("Дата выполнения не может быть в прошлом")
                .When(x => x.DueDate.HasValue);
        }
    }
}
