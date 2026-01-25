using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();
        builder.Property(t => t.IssueId).ValueGeneratedNever();
        builder.Property(t => t.UploaderId).ValueGeneratedNever();
        builder.Property(t => t.StoragePath).HasMaxLength(2000);

        builder.HasOne<User>()
              .WithMany()
              .HasForeignKey(t => t.UploaderId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Issue>()
              .WithMany()
              .HasForeignKey(t => t.IssueId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

    }
}
