using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

namespace MoodCat.App.Core.Infrastructure.Data.Interceptors;

/// <summary>
/// Interceptor-dispatcher dla eventów domenowych
/// </summary>
/// <remarks>
/// Ten interceptor jest używany do dispatchowania eventów domenowych
/// </remarks>
public class DispatchDomainEventsInterceptor(IMediator mediator) 
    : SaveChangesInterceptor
{
    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await DispatchDomainEvents(eventData.Context); 
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private async Task DispatchDomainEvents(DbContext? eventDataContext)
    {
        if (eventDataContext == null)
            return;
        
        var aggregates = eventDataContext.ChangeTracker.Entries<IAggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToArray();
        
        var domainEvents = aggregates.SelectMany(x => x.DomainEvents).ToArray();
        
        aggregates.ToList().ForEach(x => x.ClearDomainEvents());
        
        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
    
    
}