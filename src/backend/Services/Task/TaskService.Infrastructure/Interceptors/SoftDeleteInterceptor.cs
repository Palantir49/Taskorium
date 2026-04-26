using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Infrastructure.Interceptors
{
    public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null) return ValueTask.FromResult(result);

            foreach (var entry in eventData.Context.ChangeTracker.Entries<ISoftDeletable>())
            {
                if (entry.State != EntityState.Deleted) continue;

                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;

                CascadeSoftDelete(eventData.Context, entry.Entity);
            }

            return ValueTask.FromResult(result);
        }

        private static void CascadeSoftDelete(DbContext context, ISoftDeletable parentEntity)
        {
            var parentEntry = context.Entry(parentEntity);

            foreach (var navigation in parentEntry.Navigations)
            {
                if (navigation.CurrentValue is null) continue;

                if (navigation.CurrentValue is IEnumerable<ISoftDeletable> children)
                {
                    foreach (var child in children)
                    {
                        if (child.IsDeleted) continue;
                        child.IsDeleted = true;
                        child.DeletedAt = DateTime.UtcNow;
                        context.Entry(child).State = EntityState.Modified;
                    }
                }
                else if (navigation.CurrentValue is ISoftDeletable child && !child.IsDeleted)
                {
                    child.IsDeleted = true;
                    child.DeletedAt = DateTime.UtcNow;
                    context.Entry(child).State = EntityState.Modified;
                }
            }
        }
    }
}
