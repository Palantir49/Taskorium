using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.IssueId).ValueGeneratedNever();

        builder.Property(t => t.UploaderId).ValueGeneratedNever();

        builder.Property(t => t.FileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.ContentType)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.ContentLength)
            .IsRequired();

        builder.Property(t => t.StoragePath)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasOne<User>()
              .WithMany()
              .HasForeignKey(t => t.UploaderId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Issue>()
              .WithMany(x => x.Attachments)
              .HasForeignKey(t => t.IssueId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter("SoftDelete", p => !p.IsDeleted);
    }
}
