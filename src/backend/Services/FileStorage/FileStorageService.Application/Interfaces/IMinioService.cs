using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorageService.Application
{
    public interface IMinioService
    {
        Task<string> UploadFileAsync(Stream fileStream, string bucketName, string objectName, string contentType, CancellationToken cancellationToken = default);
        Task<Stream> DownloadFileAsync(string bucketName, string objectName, CancellationToken cancellationToken = default);
        Task<bool> DeleteFileAsync(string bucketName, string objectName, CancellationToken cancellationToken = default);
        Task<string> GetPresignedUrlAsync(string bucketName, string objectName, int expirySeconds, CancellationToken cancellationToken = default);
        Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken = default);
    }
}
