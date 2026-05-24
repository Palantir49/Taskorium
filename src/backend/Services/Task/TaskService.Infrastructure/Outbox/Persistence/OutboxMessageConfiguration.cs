using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Infrastructure.Outbox.Models;

namespace TaskService.Infrastructure.Outbox.Persistence;

/// <summary>
/// Конфигурация схемы таблицы OutboxMessage
/// </summary>
public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    /// <summary>
    /// Настроить отображение <see cref="OutboxMessage"/> в БД.
    /// </summary>
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessage");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.OccurredOnUtc)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Payload)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Retries)
            .IsRequired();

        builder.Property(x => x.ProcessedOnUtc);

        builder.Property(x => x.LastError)
            .HasMaxLength(4000);

        // Основной индекс для фонового процессора:
        // быстро выбирает Pending/Failed в порядке появления.
        builder.HasIndex(x => new { x.Status, x.OccurredOnUtc });
    }
}
