using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Configurations;

public class WorkspaceMemberConfiguration : IEntityTypeConfiguration<WorkspaceMember>
{
    public void Configure(EntityTypeBuilder<WorkspaceMember> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.HasKey(x => x.WorkspaceId);

        builder.Property(x => x.JoinedAt).IsRequired();

        builder.HasOne<User>()
            .WithMany(x => x.WorkspaceMembers)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Workspace>()
            .WithMany(x => x.WorkspaceMembers)
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion<string>();

        builder.HasData(FakeDataFactory.WorkspaceMembers);
    }
}
