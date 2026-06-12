using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.Attachments.Dto;

namespace TaskService.Application.Validators.Dto
{
    public class AttachmentDtoValidator : AbstractValidator<AttachmentDto>
    {
        private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/webp",
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/vnd.ms-excel",
            "text/plain",
            "application/zip",
            "application/x-rar-compressed"
        };

        private const long MaxFileSize = 50 * 1024 * 1024; // 50 МБ

        public AttachmentDtoValidator()
        {
            // Имя файла
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Имя файла обязательно")
                .MaximumLength(255)
                    .WithMessage("Имя файла не может быть длиннее 255 символов")
                .Matches(@"^[^<>:""/\\|?*\x00-\x1F]+$")
                    .WithMessage("Имя файла содержит недопустимые символы");

            // Контент (Stream) — проверяем только на null, т.к. Stream нельзя валидировать через FluentValidation
            RuleFor(x => x.Content)
                .NotNull()
                    .WithMessage("Содержимое файла обязательно");

            // Размер файла
            RuleFor(x => x.ContentLength)
                .GreaterThan(0)
                    .WithMessage("Размер файла должен быть больше 0")
                .LessThanOrEqualTo(MaxFileSize).
                    WithMessage($"Максимальный размер файла - {MaxFileSize / 1024 / 1024} МБ");

            // Тип контента
            RuleFor(x => x.ContentType)
                .NotEmpty()
                    .WithMessage("Тип контента обязателен")
                .Must(type => AllowedContentTypes.Contains(type))
                    .WithMessage($"Недопустимый тип файла. Разрешённые типы: {string.Join(", ", AllowedContentTypes)}");
        }
    }
}
