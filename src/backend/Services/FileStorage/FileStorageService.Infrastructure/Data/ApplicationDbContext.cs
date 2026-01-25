using System.Collections.Generic;
using System.Reflection.Emit;
using FileStorageService.Domain;
using Microsoft.EntityFrameworkCore;

namespace FileStorageService.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<FileMetadata> FileMetadata { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FileMetadata>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BucketName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ObjectName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Hash).HasMaxLength(100);

            entity.HasIndex(e => e.UploadedBy);
            entity.HasIndex(e => e.Hash);
            entity.HasIndex(e => e.UploadedAt);
        });
    }
}
