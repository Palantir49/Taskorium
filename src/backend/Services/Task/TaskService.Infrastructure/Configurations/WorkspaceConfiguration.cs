using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Configurations
{
    internal class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id).ValueGeneratedNever();

            builder.Property(w => w.Name)
                .HasConversion(name => name.Value, value => new BaseEntityName(value))
                .IsRequired()
                .HasMaxLength(225);

            builder.Property(w => w.CreatedDate).IsRequired();

            builder.Property(w => w.OwnerId);
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(w => w.OwnerId);
        }
    }
}
