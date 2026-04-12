using FileStorageService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace FileStorageService.Infrastructure.MinIO;

public class MinioService(IMinioClient minioClient, ILogger<MinioService> logger) : IMinioService
{
    public async Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var found = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                logger.LogInformation("Бакет {BucketName} успешно создан", bucketName);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при проверке существования бакета {BucketName}", bucketName);
            throw;
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string bucketName, string objectName,
        string contentType, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureBucketExistsAsync(bucketName, cancellationToken);
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

            var result = await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            logger.LogInformation("Вложение {ObjectName} загружено в бакет {BucketName}", objectName, bucketName);
            return result.ObjectName;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading file {ObjectName} to bucket {BucketName}", objectName, bucketName);
            throw;
        }

        finally
        {
            await fileStream.DisposeAsync();
        }
    }

    public async Task UploadFileAsync(ReadOnlyMemory<byte> file, string key, string bucketName,
        CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(bucketName, cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(key)
            .WithRequestBody(file)
            .WithObjectSize(file.Length);

        await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
    }

    public async Task<Stream> DownloadFileAsync(
        string bucketName,
        string objectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var memoryStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                });

            await minioClient.GetObjectAsync(getObjectArgs, cancellationToken);

            memoryStream.Position = 0;

            logger.LogInformation(
                "Файл {ObjectName} загружен из бакета {BucketName}",
                objectName,
                bucketName);

            return memoryStream;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ошибка при загрузке файла {ObjectName} из бакета {BucketName}",
                objectName,
                bucketName);

            throw;
        }
    }


    public async Task<bool> DeleteFileAsync(string bucketName, string objectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);

            await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            logger.LogInformation("Файл {ObjectName} удален из бакета {BucketName}", objectName, bucketName);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при удалении файла {ObjectName} из бакета {BucketName}", objectName,
                bucketName);
            return false;
        }
    }

    public async Task<string> GetPresignedUrlAsync(string bucketName, string objectName,
        int expirySeconds, CancellationToken cancellationToken = default)
    {
        try
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(expirySeconds);

            var url = await minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);

            logger.LogInformation("Presigned URL сгенерирован для {ObjectName} в бакете {BucketName}",
                objectName, bucketName);
            return url;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при генерации presigned URL для {ObjectName} в бакете {BucketName}",
                objectName, bucketName);
            throw;
        }
    }
}
