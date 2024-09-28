using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

namespace MoodCat.App.Core.Infrastructure.Data.Interceptors;

/// <summary>
/// Interceptor EF core dla auditable entities
/// </summary>
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntites(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateEntites(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntites(DbContext? eventDataContext)
    {
        if (eventDataContext == null)
            return;

        var entries = eventDataContext.ChangeTracker.Entries<IEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "system";
                    break;
                case { } when entry.HasChangedOwnedEntities() || entry.State is EntityState.Modified:
                    entry.Entity.LastModifiedAt = DateTime.Now;
                    entry.Entity.LastModifiedBy = "system";
                    break;
                default:
                    break;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => r.TargetEntry != null && r.TargetEntry.Metadata.IsOwned()
                                                        && r.TargetEntry.State is EntityState.Added
                                                            or EntityState.Modified);
}