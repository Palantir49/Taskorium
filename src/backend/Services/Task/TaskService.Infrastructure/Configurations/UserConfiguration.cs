using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Configurations
{
    //internal class UserConfiguration : IEntityTypeConfiguration<User>
    //{
    //    public void Configure(EntityTypeBuilder<User> builder)
    //    {
    //        builder.HasKey(t => t.Id);
    //        builder.Property(t => t.Id).ValueGeneratedNever();

    //        builder.Property(t => t.Email).HasConversion(
    //            email => email.ToString(),
    //            value => new EmailAdress(value))
    //            .IsRequired().HasMaxLength(225);

    //        //builder.Property(t => t.Description);
    //        //builder.Property(t => t.CreatedDate).IsRequired();
    //        //builder.Property(t => t.WorkspaceId).IsRequired();
    //        //builder.Property(t => t.StartDate);
    //        //builder.Property(t => t.FinishDate);
    //    }
    //}
}
