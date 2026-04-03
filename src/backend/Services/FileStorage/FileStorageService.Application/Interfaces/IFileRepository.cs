using FileStorageService.Domain.Entities;

namespace FileStorageService.Application.Interfaces;

public interface IFileRepository
{
    Task<FileMetadata> CreateAsync(FileMetadata fileMetadata, CancellationToken cancellationToken = default);
    Task<FileMetadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FileMetadata>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task UpdateAsync(FileMetadata fileMetadata, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
