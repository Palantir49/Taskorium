using System.Security.Cryptography;
using FileStorageService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileStorageService.Application;

public class FileStorageService : IFileStorageService
{
    private readonly IMinioService _minioService;
    private readonly IFileRepository _fileRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(
        IMinioService minioService,
        IFileRepository fileRepository,
        IConfiguration configuration,
        ILogger<FileStorageService> logger)
    {
        _minioService = minioService;
        _fileRepository = fileRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<FileUploadResponseDto> UploadFileAsync(FileUploadDto fileUpload, Guid userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketName = fileUpload.BucketName ?? _configuration["MinIO:DefaultBucket"]!;
            var fileId = Guid.NewGuid();
            var extension = Path.GetExtension(fileUpload.File.FileName);
            var objectName = $"{userId}/{fileId}{extension}";

            using var stream = fileUpload.File.OpenReadStream();
            var hash = await ComputeHashAsync(stream);
            stream.Position = 0;

            // Upload to MinIO
            await _minioService.UploadFileAsync(
                stream,
                bucketName,
                objectName,
                fileUpload.File.ContentType,
                cancellationToken);

            // Save metadata to database
            var metadata = new FileMetadata
            {
                Id = fileId,
                FileName = objectName,
                OriginalFileName = fileUpload.File.FileName,
                ContentType = fileUpload.File.ContentType,
                Size = fileUpload.File.Length,
                BucketName = bucketName,
                ObjectName = objectName,
                Description = fileUpload.Description,
                Tags = fileUpload.Tags,
                UploadedBy = userId,
                UploadedAt = DateTime.UtcNow,
                Status = FileStatus.Available,
                Hash = hash
            };

            await _fileRepository.CreateAsync(metadata, cancellationToken);

            _logger.LogInformation("File {FileId} uploaded successfully by user {UserId}", fileId, userId);

            return new FileUploadResponseDto
            {
                FileId = fileId,
                FileName = fileUpload.File.FileName,
                Size = fileUpload.File.Length,
                ContentType = fileUpload.File.ContentType,
                UploadedAt = metadata.UploadedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file for user {UserId}", userId);
            throw;
        }
    }

    public async Task<FileDownloadDto> DownloadFileAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var metadata = await _fileRepository.GetByIdAsync(fileId, cancellationToken);

        if (metadata == null || metadata.IsDeleted)
        {
            throw new FileNotFoundException($"File {fileId} not found");
        }

        var stream = await _minioService.DownloadFileAsync(metadata.BucketName, metadata.ObjectName, cancellationToken);

        return new FileDownloadDto
        {
            FileStream = stream,
            FileName = metadata.OriginalFileName,
            ContentType = metadata.ContentType,
            Size = metadata.Size
        };
    }

    public async Task<bool> DeleteFileAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var metadata = await _fileRepository.GetByIdAsync(fileId, cancellationToken);

        if (metadata == null)
        {
            return false;
        }

        // Soft delete in database
        metadata.IsDeleted = true;
        metadata.Status = FileStatus.Deleted;
        metadata.ModifiedAt = DateTime.UtcNow;
        await _fileRepository.UpdateAsync(metadata, cancellationToken);

        // Delete from MinIO
        await _minioService.DeleteFileAsync(metadata.BucketName, metadata.ObjectName, cancellationToken);

        _logger.LogInformation("File {FileId} deleted successfully", fileId);
        return true;
    }

    public async Task<FileMetadataDto?> GetFileMetadataAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var metadata = await _fileRepository.GetByIdAsync(fileId, cancellationToken);

        if (metadata == null || metadata.IsDeleted)
        {
            return null;
        }

        return new FileMetadataDto
        {
            Id = metadata.Id,
            FileName = metadata.FileName,
            OriginalFileName = metadata.OriginalFileName,
            ContentType = metadata.ContentType,
            Size = metadata.Size,
            Description = metadata.Description,
            Tags = metadata.Tags,
            UploadedAt = metadata.UploadedAt,
            Status = metadata.Status.ToString()
        };
    }

    public async Task<IEnumerable<FileMetadataDto>> GetUserFilesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var files = await _fileRepository.GetByUserIdAsync(userId, cancellationToken);

        return files.Where(f => !f.IsDeleted).Select(f => new FileMetadataDto
        {
            Id = f.Id,
            FileName = f.FileName,
            OriginalFileName = f.OriginalFileName,
            ContentType = f.ContentType,
            Size = f.Size,
            Description = f.Description,
            Tags = f.Tags,
            UploadedAt = f.UploadedAt,
            Status = f.Status.ToString()
        });
    }

    public async Task<PresignedUrlDto> GetPresignedUrlAsync(Guid fileId, int expiryMinutes = 60,
        CancellationToken cancellationToken = default)
    {
        var metadata = await _fileRepository.GetByIdAsync(fileId, cancellationToken);

        if (metadata == null || metadata.IsDeleted)
        {
            throw new FileNotFoundException($"File {fileId} not found");
        }

        var url = await _minioService.GetPresignedUrlAsync(
            metadata.BucketName,
            metadata.ObjectName,
            expiryMinutes * 60,
            cancellationToken);

        return new PresignedUrlDto
        {
            Url = url,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
        };
    }

    private async Task<string> ComputeHashAsync(Stream stream)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = await sha256.ComputeHashAsync(stream);
        return Convert.ToBase64String(hashBytes);
    }
}
