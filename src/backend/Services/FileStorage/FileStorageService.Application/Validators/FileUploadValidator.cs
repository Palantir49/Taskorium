using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace FileStorageService.Application;

public class FileUploadValidator : AbstractValidator<FileUploadDto>
{
    private readonly IConfiguration _configuration;

    public FileUploadValidator(IConfiguration configuration)
    {
        _configuration = configuration;

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(_configuration.GetValue<long>("FileStorage:MaxFileSizeMB") * 1024 * 1024)
            .WithMessage($"File size must not exceed {_configuration.GetValue<long>("FileStorage:MaxFileSizeMB")} MB");

        RuleFor(x => x.File.FileName)
            .Must(BeAllowedExtension)
            .WithMessage("File extension is not allowed");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters");
    }

    private bool BeAllowedExtension(string fileName)
    {
        var allowedExtensions = _configuration.GetSection("FileStorage:AllowedExtensions").Get<string[]>();
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return allowedExtensions?.Contains(extension) ?? false;
    }
}
