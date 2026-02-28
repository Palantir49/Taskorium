using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;

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
        //TODO: если будет нужен, то сделать обязательным или в сущности добавить дефолт
        builder.Property(t => t.Color);

        builder.HasOne<Project>()
              .WithMany(x => x.Statuses)
              .HasForeignKey(t => t.ProjectId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);
    }
}
