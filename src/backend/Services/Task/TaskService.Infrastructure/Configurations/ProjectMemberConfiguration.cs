using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Configurations;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.HasKey(t => new { t.ProjectId, t.UserId });

        builder.Property(t => t.ProjectId)
            .ValueGeneratedNever()
            .IsRequired(); ;

        builder.Property(t => t.UserId)
            .ValueGeneratedNever()
            .IsRequired(); ;

        builder.Property(t => t.Role)
            .HasConversion<string>();

        builder.Property(t => t.JoinedAt)
            .IsRequired(); ;

        builder.HasOne<Project>()
            .WithMany(x => x.ProjectMembers)
            .HasForeignKey(t => t.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany(x => x.ProjectMembers)
            .HasForeignKey(t => t.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasData(FakeDataFactory.ProjectMembers);
    }
}
