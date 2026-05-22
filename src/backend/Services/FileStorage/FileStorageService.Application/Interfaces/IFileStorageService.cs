namespace FileStorageService.Application.Interfaces;

public interface IFileStorageService
{
    Task UploadFileAsync(ReadOnlyMemory<byte> content, string key,
        CancellationToken cancellationToken = default);

    Task UploadFileAsync(Stream content, string key, string contentType,
        CancellationToken cancellationToken = default);

    Task DeleteFileAsync(string key,
        CancellationToken cancellationToken = default);

    Task<FileDownloadDto> DownloadFileAsync(string key, CancellationToken cancellationToken = default);
}
