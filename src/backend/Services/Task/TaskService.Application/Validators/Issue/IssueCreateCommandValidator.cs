using FluentValidation;
using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Validators.Dto;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Issues.Command
{
    public class IssueCreateCommandValidator : AbstractValidator<IssueCreateCommand>
    {
        public IssueCreateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Название задачи обязательно")
                .MinimumLength(3)
                    .WithMessage("Название должно содержать минимум 3 символа")
                .MaximumLength(255)
                    .WithMessage("Название не может быть длиннее 255 символов")
                .Matches(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\-_.,!?() ]+$")
                    .WithMessage("Название содержит недопустимые символы");

            RuleFor(x => x.ProjectId)
                .NotEqual(Guid.Empty)
                    .WithMessage("ID проекта не может быть пустым");
            RuleFor(x => x.IssueType)
                .Must(x => Enum.IsDefined(x))
                    .WithMessage("Недопустимый тип задачи");

            // Приоритет задачи
            RuleFor(x => x.IssuePriority)
                .Must(x => Enum.IsDefined(x))
                    .WithMessage("Недопустимый приоритет задачи");

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

            // Вложения
            RuleFor(x => x.AttachmentDtos)
                .Must(list => list == null || list.Count <= 10)
                    .WithMessage("Максимальное количество вложений - 10")
                .When(x => x.AttachmentDtos != null);

            RuleForEach(x => x.AttachmentDtos)
                .SetValidator(new AttachmentDtoValidator());

            // Исполнители
            RuleFor(x => x.AssigneeDtos)
                .Must(list => list == null || list.Count <= 50)
                    .WithMessage("Максимальное количество исполнителей - 50")
                .When(x => x.AssigneeDtos != null);

            RuleFor(x => x.AssigneeDtos)
                .Must(list => list == null || list.Select(a => a.UserId).Distinct().Count() == list.Count)
                    .WithMessage("Исполнители не должны повторяться")
                .When(x => x.AssigneeDtos != null);

            RuleForEach(x => x.AssigneeDtos)
                .SetValidator(new IssueAssigneesDtoValidator());
        }
    }
}
