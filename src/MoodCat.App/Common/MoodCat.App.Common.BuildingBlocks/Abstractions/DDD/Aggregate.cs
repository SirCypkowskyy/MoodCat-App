namespace MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

/// <summary>
/// Abstrakcyjna klasa bazowa dla agregatów
/// </summary>
/// <remarks>
/// Agregat jest to obiekt, który jest odpowiedzialny za zarządzanie innymi obiektami w swoim kontekście.
/// Dzięki temu, że agregat jest odpowiedzialny za zarządzanie innymi obiektami, to może on zarządzać
/// zdarzeniami domenowymi, które są związane z tymi obiektami.
/// </remarks>
/// <typeparam name="TKey">
/// Typ klucza głównego agregatu
/// </typeparam>
public abstract class Aggregate<TKey> : Entity<TKey>, IAggregate<TKey>
where TKey : notnull
{
    /// <summary>
    /// Lista zdarzeń domenowych
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Zwraca listę zdarzeń domenowych w postaci listy tylko do odczytu
    /// </summary>
    /// <remarks>
    /// Zdarzenia domenowe są zwracane w postaci listy tylko do odczytu, aby nie można było modyfikować
    /// listy zdarzeń domenowych spoza klasy <see cref="Aggregate{TKey}"/>.
    /// </remarks>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    /// <summary>
    /// Dodaje zdarzenie domenowe do listy zdarzeń domenowych
    /// </summary>
    /// <remarks>
    /// Zdarzenia domenowe są dodawane do listy zdarzeń domenowych, aby można było je później wyczyścić.
    /// Dla każdego agregatu można dodać wiele zdarzeń domenowych.
    /// <br />
    /// Dzięki temu, że zdarzenia domenowe są dodawane do listy, można je później wyczyścić, aby nie były
    /// one przetwarzane wielokrotnie.
    /// <br />
    /// Zdarzenia domenowe są dodawane do listy w kolejności, w jakiej zostały dodane do listy.
    /// <br />
    /// Zdarzenia domenowe są dodawane do listy w sposób synchroniczny, co oznacza, że nie ma potrzeby
    /// synchronizacji dostępu do listy zdarzeń domenowych.
    /// </remarks>
    /// <param name="domainEvent">
    /// Zdarzenie domenowe, które ma zostać dodane do listy zdarzeń domenowych
    /// </param>
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    /// <inheritdoc />
    public IDomainEvent[] ClearDomainEvents()
    {
        var domainEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return domainEvents;
    }
}