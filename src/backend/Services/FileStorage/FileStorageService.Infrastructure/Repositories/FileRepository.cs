using FileStorageService.Application;
using FileStorageService.Domain;
using Microsoft.EntityFrameworkCore;

namespace FileStorageService.Infrastructure;

public class FileRepository : IFileRepository
{
    private readonly ApplicationDbContext _context;

    public FileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FileMetadata> CreateAsync(FileMetadata fileMetadata, CancellationToken cancellationToken = default)
    {
        _context.FileMetadata.Add(fileMetadata);
        await _context.SaveChangesAsync(cancellationToken);
        return fileMetadata;
    }

    public async Task<FileMetadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.FileMetadata
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FileMetadata>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.FileMetadata
            .Where(f => f.UploadedBy == userId)
            .OrderByDescending(f => f.UploadedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(FileMetadata fileMetadata, CancellationToken cancellationToken = default)
    {
        _context.FileMetadata.Update(fileMetadata);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await GetByIdAsync(id, cancellationToken);
        if (file != null)
        {
            _context.FileMetadata.Remove(file);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
