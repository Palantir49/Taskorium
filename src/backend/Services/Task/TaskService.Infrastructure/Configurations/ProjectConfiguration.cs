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
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.Property(p => p.Name).HasConversion(
                name => name.ToString(),
                value => new BaseEntityName(value))
                .IsRequired().HasMaxLength(225);

            builder.Property(p => p.Description);

            builder.Property(p => p.CreatedDate).IsRequired();

            builder.Property(p => p.WorkspaceId).IsRequired();
            builder.HasOne<Workspace>()
                .WithMany()
                .HasForeignKey(p => p.WorkspaceId);

            builder.Property(p => p.StartDate);

            builder.Property(p => p.FinishDate);
        }
    }
}
