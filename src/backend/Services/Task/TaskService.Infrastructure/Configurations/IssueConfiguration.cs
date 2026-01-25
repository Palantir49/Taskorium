using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Configurations
{
    internal class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Property(t => t.Name).HasConversion(
                name => name.Value,
                value => new BaseEntityName(value))
                .IsRequired().HasMaxLength(225);

            builder.Property(t => t.Description).HasMaxLength(2000);

            builder.Property(t => t.CreatedDate).IsRequired();
            builder.Property(t => t.UpdatedDate);
            builder.Property(t => t.DueDate);
            builder.Property(t => t.ResolvedDate);

            builder.Property(t => t.ProjectId).IsRequired();
            builder.Property(t => t.IssueStatusId).IsRequired();
            builder.Property(t => t.IssueTypeId).IsRequired();

            builder.HasOne<Project>()
                  .WithMany()
                  .HasForeignKey(t => t.ProjectId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(i => i.User)
            //    .WithMany(u => u.Issues)
            //    .HasForeignKey(i => i.ReporterId);

        }
    }
}
