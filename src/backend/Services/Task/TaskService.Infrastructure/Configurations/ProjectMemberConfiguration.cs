using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Configurations;

public class ProjectMemberConfiguration
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.HasKey(t => t.ProjectId);

        builder.Property(t => t.ProjectId).ValueGeneratedNever();
        builder.HasKey(t => t.UserId);
        builder.Property(t => t.UserId).ValueGeneratedNever();

        builder.Property(t => t.Role);
        builder.Property(t => t.JoinedAt);

        builder.HasOne<Project>()
              .WithMany()
              .HasForeignKey(t => t.ProjectId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
              .WithMany()
              .HasForeignKey(t => t.UserId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

    }
}
