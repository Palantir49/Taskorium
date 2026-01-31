using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Configurations;

internal class IssueStatusConfiguration : IEntityTypeConfiguration<IssueStatus>
{
    public void Configure(EntityTypeBuilder<IssueStatus> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).ValueGeneratedNever();
        builder.Property(t => t.ProjectId).IsRequired();
        builder.Property(t=>t.Type).IsRequired();
        builder.Property(t=>t.Position).IsRequired();
        //TODO: если будет нужен, то сделать обязательным или в сущности добавить дефолт
        builder.Property(t=>t.Color);
    }
}
