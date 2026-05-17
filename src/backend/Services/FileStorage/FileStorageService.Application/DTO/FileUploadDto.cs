using Microsoft.AspNetCore.Http;

namespace FileStorageService.Application.DTO;

public class FileUploadDto
{
    public IFormFile File { get; set; } = null!;
    public string? Description { get; set; }
    public string? Tags { get; set; }
    public string? BucketName { get; set; }
}
