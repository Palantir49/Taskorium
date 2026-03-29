using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Configurations;

internal class IssueStatusConfiguration : IEntityTypeConfiguration<IssueStatus>
{
    public void Configure(EntityTypeBuilder<IssueStatus> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Name).HasConversion(
            name => name.Value,
            value => new BaseEntityName(value))
            .IsRequired().HasMaxLength(225);

        builder.Property(t => t.ProjectId).IsRequired();
        builder.Property(t => t.Type).IsRequired();
        builder.Property(t => t.Position).IsRequired();
        builder.Property(t => t.CreatedDate).IsRequired();

        builder.HasOne<Project>()
              .WithMany(x => x.Statuses)
              .HasForeignKey(t => t.ProjectId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);
        builder.HasData(FakeDataFactory.IssueStatuses);
    }
}
