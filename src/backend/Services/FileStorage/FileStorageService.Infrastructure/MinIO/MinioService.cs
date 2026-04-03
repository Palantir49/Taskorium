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
                logger.LogInformation("Bucket {BucketName} created successfully", bucketName);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error ensuring bucket {BucketName} exists", bucketName);
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

            logger.LogInformation("File {ObjectName} uploaded to bucket {BucketName}", objectName, bucketName);
            return result.ObjectName;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading file {ObjectName} to bucket {BucketName}", objectName, bucketName);
            throw;
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

        var result = await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
    }

    public async Task<byte[]> DownloadFileAsync(string bucketName, string objectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var memoryStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(async void (stream) =>
                {
                    await stream.CopyToAsync(memoryStream, cancellationToken);
                });

            await minioClient.GetObjectAsync(getObjectArgs, cancellationToken);

            //memoryStream.Position = 0;
            logger.LogInformation("File {ObjectName} downloaded from bucket {BucketName}", objectName, bucketName);
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error downloading file {ObjectName} from bucket {BucketName}", objectName, bucketName);
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

            logger.LogInformation("File {ObjectName} deleted from bucket {BucketName}", objectName, bucketName);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting file {ObjectName} from bucket {BucketName}", objectName, bucketName);
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

            logger.LogInformation("Presigned URL generated for {ObjectName} in bucket {BucketName}",
                objectName, bucketName);
            return url;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating presigned URL for {ObjectName} in bucket {BucketName}",
                objectName, bucketName);
            throw;
        }
    }
}
