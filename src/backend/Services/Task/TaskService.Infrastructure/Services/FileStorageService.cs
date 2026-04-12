using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Taskorium.FileStorage.V1;

namespace TaskService.Infrastructure.Services;

/// <summary>
///     Обеспечивает операции хранения файлов, включая загрузку, выгрузку и удаление файлов
///     через gRPC-клиент к сервису хранения файлов Taskorium.
/// </summary>
public class FileStorageService(
    Taskorium.FileStorage.V1.FileStorageService.FileStorageServiceClient fileStorageClient,
    ILogger<FileStorageService> logger)
{
    /// <summary>
    ///     Загружает файл из сервиса хранения по его имени.
    /// </summary>
    /// <param name="name">Имя/идентификатор файла для загрузки.</param>
    /// <param name="token">Токен отмены для прерывания операции.</param>
    /// <returns>Задача, представляющая асинхронную операцию, содержащая содержимое файла как ReadOnlyMemory<byte>.</returns>
    public async Task<ReadOnlyMemory<byte>> DownloadAsync(string name, CancellationToken token = default)
    {
        logger.LogInformation("Начало отправки запроса на получение вложения {ObjectName} из хранилища.", name);
        var request = new GetFileRequest { Name = name };
        var file = await fileStorageClient.DownloadFileAsync(request, cancellationToken: token);
        logger.LogInformation("Получено вложение {ObjectName} из хранилища.", name);
        return file.Body.Memory;
    }

    /// <summary>
    ///     Загружает файл в сервис хранения с указанным именем, типом содержимого и потоком данных.
    /// </summary>
    /// <param name="name">Имя/идентификатор, который будет присвоен загружаемому файлу.</param>
    /// <param name="contentType">MIME-тип содержимого файла.</param>
    /// <param name="stream">Поток, содержащий содержимое файла для загрузки.</param>
    /// <param name="token">Токен отмены для прерывания операции.</param>
    /// <returns>Задача, представляющая асинхронную операцию загрузки.</returns>
    public async Task UploadAsync(string name, string contentType, Stream stream, CancellationToken token = default)
    {
        logger.LogInformation("Начало отправки запроса на загрузку вложения {ObjectName} в хранилище.", name);
        var contentStream = await ByteString.FromStreamAsync(stream, token);
        var request = new UploadFileRequest { Name = name, Body = contentStream, ContentType = contentType };
        await fileStorageClient.UploadFileAsync(request, cancellationToken: token);
        logger.LogInformation("Завершена загрузка вложения {ObjectName} в хранилище.", name);
    }

    /// <summary>
    ///     Удаляет файл из сервиса хранения по его имени.
    /// </summary>
    /// <param name="name">Имя/идентификатор файла для удаления.</param>
    /// <param name="token">Токен отмены для прерывания операции.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления.</returns>
    public async Task DeleteAsync(string name, CancellationToken token = default)
    {
        logger.LogInformation("Начало отправки запроса на удаление вложения {ObjectName} из хранилища.", name);
        var request = new DeleteFileRequest { Name = name };
        await fileStorageClient.DeleteFileAsync(request, cancellationToken: token);
        logger.LogInformation("Завершено удаление вложения {ObjectName} из хранилища.", name);
    }
}
