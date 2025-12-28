using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Configurations
{
    internal class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).ValueGeneratedNever();
   
            builder.Property(t => t.Name).IsRequired().HasMaxLength(225);
            
            builder.Property(t => t.CreatedDate).IsRequired();
            
            builder.Property(t => t.OwnerId);

        }
    }
}
