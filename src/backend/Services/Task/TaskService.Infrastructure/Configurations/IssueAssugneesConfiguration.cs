using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Configurations;

public class IssueAssugneesConfiguration : IEntityTypeConfiguration<IssueAssignees>
{
    public void Configure(EntityTypeBuilder<IssueAssignees> builder)
    {
        builder.HasKey(k => new { k.UserId, k.IssueId });

        builder.Property(p => p.UserId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(p => p.IssueId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(p => p.Role)
            .IsRequired();

        builder.HasOne(x => x.User)
              .WithMany(x => x.issueAssignees)
              .HasForeignKey(t => t.UserId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(x => x.Issue)
              .WithMany(x => x.IssueAssignees)
              .HasForeignKey(t => t.IssueId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter("SoftDelete", p => !p.IsDeleted);
    }
}
