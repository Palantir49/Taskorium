using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorageService.Application
{
    public interface IFileStorageService
    {
        Task<FileUploadResponseDto> UploadFileAsync(FileUploadDto fileUpload, Guid userId, CancellationToken cancellationToken = default);
        Task<FileDownloadDto> DownloadFileAsync(Guid fileId, CancellationToken cancellationToken = default);
        Task<bool> DeleteFileAsync(Guid fileId, CancellationToken cancellationToken = default);
        Task<FileMetadataDto?> GetFileMetadataAsync(Guid fileId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FileMetadataDto>> GetUserFilesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<PresignedUrlDto> GetPresignedUrlAsync(Guid fileId, int expiryMinutes = 60, CancellationToken cancellationToken = default);
    }
}
