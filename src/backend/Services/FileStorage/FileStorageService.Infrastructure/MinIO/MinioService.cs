using FileStorageService.Application;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace FileStorageService.Infrastructure;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioService> _logger;

    public MinioService(IMinioClient minioClient, ILogger<MinioService> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                _logger.LogInformation("Bucket {BucketName} created successfully", bucketName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ensuring bucket {BucketName} exists", bucketName);
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

            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            _logger.LogInformation("File {ObjectName} uploaded to bucket {BucketName}", objectName, bucketName);
            return result.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {ObjectName} to bucket {BucketName}", objectName, bucketName);
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsync(string bucketName, string objectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var memoryStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(async (stream) =>
                {
                    await stream.CopyToAsync(memoryStream, cancellationToken);
                });

            await _minioClient.GetObjectAsync(getObjectArgs, cancellationToken);

            memoryStream.Position = 0;
            _logger.LogInformation("File {ObjectName} downloaded from bucket {BucketName}", objectName, bucketName);
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {ObjectName} from bucket {BucketName}", objectName, bucketName);
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

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            _logger.LogInformation("File {ObjectName} deleted from bucket {BucketName}", objectName, bucketName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {ObjectName} from bucket {BucketName}", objectName, bucketName);
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

            string url = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);

            _logger.LogInformation("Presigned URL generated for {ObjectName} in bucket {BucketName}",
                objectName, bucketName);
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating presigned URL for {ObjectName} in bucket {BucketName}",
                objectName, bucketName);
            throw;
        }
    }
}
