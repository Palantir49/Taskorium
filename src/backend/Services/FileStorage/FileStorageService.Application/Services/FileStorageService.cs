using FileStorageService.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FileStorageService.Application.Services;

public class FileStorageService
    : IFileStorageService
{
    private readonly string _bucketName;
    private readonly IMinioService _minioService;

    public FileStorageService(IMinioService minioService, IConfiguration configuration)
    {
        _minioService = minioService;
        _bucketName = configuration["MinIO:DefaultBucket"] ?? throw new ArgumentNullException(_bucketName);
    }

    public async Task UploadFileAsync(ReadOnlyMemory<byte> content, string key,
        CancellationToken cancellationToken = default)
    {
        await _minioService.UploadFileAsync(content, _bucketName, key, cancellationToken);
    }


    public async Task DeleteFileAsync(string key, CancellationToken cancellationToken = default)
    {
        await _minioService.DeleteFileAsync(_bucketName, key, cancellationToken);
    }

    public async Task<FileDownloadDto> DownloadFileAsync(string key, CancellationToken cancellationToken = default)
    {
        var stream = await _minioService.DownloadFileAsync(_bucketName, key, cancellationToken);
        return new FileDownloadDto { FileStream = stream };
    }


    public async Task UploadFileAsync(Stream content, string key, string contentType,
        CancellationToken cancellationToken = default)
    {
        await _minioService.UploadFileAsync(content, _bucketName, key, contentType, cancellationToken);
    }
}
