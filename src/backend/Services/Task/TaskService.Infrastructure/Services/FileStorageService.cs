using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Taskorium.FileStorage.V1;

namespace TaskService.Infrastructure.Services;

public class FileStorageService(
    Taskorium.FileStorage.V1.FileStorageService.FileStorageServiceClient fileStorageClient,
    ILogger<FileStorageService> logger)
{
    public async Task<ReadOnlyMemory<byte>> DownloadAsync(string name, CancellationToken token = default)
    {
        var request = new GetFileRequest { Name = name };
        var file = await fileStorageClient.DownloadFileAsync(request, cancellationToken: token);
        return file.Body.Memory;
    }


    public async Task UploadAsync(string name, string contentType, Stream stream, CancellationToken token = default)
    {
        logger.LogInformation("Начало отправки запроса на загрузку вложения {ObjectName} в хранилище.", name);
        var contentStream = await ByteString.FromStreamAsync(stream, token);
        var request = new UploadFileRequest { Name = name, Body = contentStream, ContentType = contentType };
        await fileStorageClient.UploadFileAsync(request, cancellationToken: token);
        logger.LogInformation("Конец загрузки  вложение {ObjectName} в хранилище.", name);
    }
}
