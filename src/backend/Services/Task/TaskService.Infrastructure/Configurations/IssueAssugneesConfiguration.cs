using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Configurations;

public class IssueAssugneesConfiguration : IEntityTypeConfiguration<IssueAssignees>
{
    public void Configure(EntityTypeBuilder<IssueAssignees> builder)
    {
        builder.HasKey(t => new { t.UserId, t.IssueId });

        builder.HasOne(x => x.User)
              .WithMany(x => x.issueAssignees)
              .HasForeignKey(t => t.UserId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(x => x.Issue)
              .WithMany(x => x.issueAssignees)
              .HasForeignKey(t => t.IssueId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

    }
}
