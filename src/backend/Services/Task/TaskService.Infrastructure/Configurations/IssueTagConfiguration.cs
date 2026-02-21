using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Configurations;

internal class IssueTagConfiguration : IEntityTypeConfiguration<IssueTag>
{
    public void Configure(EntityTypeBuilder<IssueTag> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        //Стоит ли делать методы расширения для VO? чтобы убрать вот эти дубли
        builder.Property(t => t.Name).HasConversion(
            name => name.Value,
            value => new BaseEntityName(value))
            .IsRequired().HasMaxLength(225);

        builder.Property(t => t.ProjectId).IsRequired();
        builder.Property(t => t.CreatedDate).IsRequired();
        builder.Property(t => t.Color);

        builder.HasOne<Project>()
              .WithMany()
              .HasForeignKey(t => t.ProjectId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);
    }

}
