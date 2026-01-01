using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Configurations
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Property(t => t.Name).HasConversion(
                name => name.ToString(),
                value => new BaseEntityName(value))
                .IsRequired().HasMaxLength(225);

            builder.Property(t => t.Description);
            builder.Property(t => t.CreatedDate).IsRequired();
            builder.Property(t => t.WorkspaceId).IsRequired();
            builder.Property(t => t.StartDate);
            builder.Property(t => t.FinishDate);
        }
    }
}
