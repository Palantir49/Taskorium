using FileStorageService.Application.Interfaces;
using FileStorageService.Domain.Entities;
using FileStorageService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FileStorageService.Infrastructure.Repositories;

public class FileRepository(ApplicationDbContext context) : IFileRepository
{
    public async Task<FileMetadata> CreateAsync(FileMetadata fileMetadata,
        CancellationToken cancellationToken = default)
    {
        context.FileMetadata.Add(fileMetadata);
        await context.SaveChangesAsync(cancellationToken);
        return fileMetadata;
    }

    public async Task<FileMetadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.FileMetadata
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FileMetadata>> GetByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await context.FileMetadata
            .Where(f => f.UploadedBy == userId)
            .OrderByDescending(f => f.UploadedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(FileMetadata fileMetadata, CancellationToken cancellationToken = default)
    {
        context.FileMetadata.Update(fileMetadata);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await GetByIdAsync(id, cancellationToken);
        if (file != null)
        {
            context.FileMetadata.Remove(file);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
