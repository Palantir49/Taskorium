using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Contracts.Enum;
using TaskService.Domain.ValueObjects;

namespace TaskService.Application.Validators.IssueStatus
{
    public class IssueStatusUpdateCommandValidator : AbstractValidator<IssueStatusUpdateCommand>
    {
        public IssueStatusUpdateCommandValidator()
        {
            // Название статуса
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Название статуса обязательно")
                .MinimumLength(2)
                    .WithMessage("Название должно содержать минимум 2 символа")
                .MaximumLength(50)
                    .WithMessage("Название не может быть длиннее 50 символов")
                .Matches(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\-_]+$")
                    .WithMessage("Название может содержать только буквы, цифры, пробелы, дефис и подчеркивание");

            RuleFor(x => x.Type)
                .Must(x => Enum.IsDefined(x))
                    .WithMessage("Недопустимый тип статуса");

            RuleFor(x => x.Position)
                .GreaterThanOrEqualTo(0).
                    WithMessage("Позиция не может быть отрицательной")
                .LessThan(1000).
                    WithMessage("Позиция не может превышать 1000");

            RuleFor(x => x.Color)
                .NotEmpty().
                    WithMessage("Цвет обязателен")
                .Must(BeValidHexColor).
                    WithMessage("Некорректный формат цвета. Используйте формат #RRGGBB или #RRGGBBAA")
                .Must(BeValidDomainColor).
                    WithMessage("Цвет должен быть в формате hex (например, #FF5733)");
        }
        private bool BeValidHexColor(string? color)
        {
            if (string.IsNullOrWhiteSpace(color))
                return false;

            color = color.TrimStart('#');
            return color.Length is 6 or 8;
        }

        private bool BeValidDomainColor(string? color)
        {
            try
            {
                DomainColor.FromHex(color!);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
