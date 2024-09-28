using MediatR;

namespace MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

/// <summary>
/// Interfejs reprezentujący domenowe zdarzenie.
/// </summary>
/// <remarks>
/// Domenowe zdarzenie jest specjalnym rodzajem notyfikacji, które
/// reprezentuje coś, co już się wydarzyło w systemie. Zdarzenia domenowe
/// są używane do komunikacji między różnymi częściami systemu, a także
/// do rejestrowania historii zmian w systemie.
/// <br />
/// Zdarzenia domenowe są jednym z podstawowych elementów modelu
/// domenowego (DDD), który pozwala na reprezentację zmian w systemie
/// w sposób zdecentralizowany i asynchroniczny, co pozwala na
/// zwiększenie wydajności i skalowalności systemu.
/// </remarks>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Identyfikator zdarzenia.
    /// </summary>
    Guid EventId => Guid.NewGuid();

    /// <summary>
    /// Data i czas wystąpienia zdarzenia.
    /// </summary>
    public DateTime OccurredOn => DateTime.Now;
    
    /// <summary>
    /// Typ zdarzenia.
    /// </summary>
    /// <remarks>
    /// Typ zdarzenia jest używany do identyfikacji zdarzenia w systemie.
    /// </remarks>
    public string EventType => GetType().AssemblyQualifiedName!;
}