using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.CreatedDate).IsRequired();

        builder.Property(t => t.KeycloakId).ValueGeneratedNever();

        builder.Property(t => t.FullName).IsRequired().HasMaxLength(100);

        builder.Property(t => t.Email).HasConversion(
                email => email.ToString(),
                value => new EmailAdress(value))
            .IsRequired().HasMaxLength(225);

        builder.Property(t => t.Username).HasConversion(
                username => username.ToString(),
                value => new UserName(value))
            .IsRequired().HasMaxLength(225);
    }
}
