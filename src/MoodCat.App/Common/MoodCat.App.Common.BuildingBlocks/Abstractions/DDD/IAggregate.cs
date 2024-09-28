namespace MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

/// <summary>
/// Interfejs reprezentujący agregat (w ramach wzorca DDD).
/// </summary>
/// <remarks>
/// Agregat jest zbiorem powiązanych ze sobą encji, które razem tworzą
/// spójną całość. Agregat charakteryzuje się tym, że posiada jedną główną
/// encję, która pełni rolę korzenia agregatu. Wszystkie operacje na
/// encjach wchodzących w skład agregatu powinny być wykonywane poprzez
/// korzeń agregatu, co zapewnia spójność danych w ramach agregatu.
/// <br />
/// Agregat jest jednym z podstawowych elementów modelu domenowego (DDD), który
/// pozwala na reprezentację bardziej złożonych struktur danych.
/// </remarks>
public interface IAggregate : IEntity
{
    /// <summary>
    /// Lista domenowych zdarzeń, które zostały zgromadzone w ramach operacji
    /// na agregacie.
    /// </summary>
    /// <remarks>
    /// Domenowe zdarzenia są zdarzeniami, które reprezentują zmiany stanu
    /// agregatu. Zdarzenia te są zbierane w ramach operacji na agregacie i
    /// mogą być wykorzystane do zapisu tych zmian w bazie danych, a także
    /// do propagacji tych zmian do innych części systemu.
    /// <br />
    /// Lista zdarzeń jest tylko do odczytu, co oznacza, że nie można dodawać
    /// nowych zdarzeń do listy po utworzeniu agregatu. Zdarzenia są dodawane
    /// do listy automatycznie w ramach operacji na agregacie.
    /// <br />
    /// Dzięki zastosowaniu domenowych zdarzeń, możliwe jest zapewnienie
    /// spójności danych w ramach systemu, a także ułatwienie implementacji
    /// mechanizmów asynchronicznej komunikacji między różnymi częściami
    /// systemu.
    /// </remarks>
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    
    /// <summary>
    /// Czyści listę domenowych zdarzeń zgromadzonych w ramach operacji na
    /// agregacie.
    /// </summary>
    /// <remarks>
    /// Metoda usuwa wszystkie zgromadzone zdarzenia z listy i zwraca je jako
    /// tablicę. Dzięki temu możliwe jest zapisanie tych zdarzeń w bazie danych
    /// lub przekazanie ich do innych części systemu.
    /// <br />
    /// Metoda jest przydatna w przypadku, gdy chcemy zapisać zdarzenia w bazie
    /// danych po wykonaniu operacji na agregacie, a także w przypadku, gdy
    /// chcemy przekazać te zdarzenia do innych części systemu.
    /// </remarks>
    /// <returns>
    /// Tablica zgromadzonych domenowych zdarzeń.
    /// </returns>
    IDomainEvent[] ClearDomainEvents();
}

/// <summary>
/// Interfejs reprezentujący agregat (w ramach wzorca DDD) z identyfikatorem.
/// </summary>
/// <remarks>
/// Agregat jest zbiorem powiązanych ze sobą encji, które razem tworzą
/// spójną całość. Agregat charakteryzuje się tym, że posiada jedną główną
/// encję, która pełni rolę korzenia agregatu. Wszystkie operacje na
/// encjach wchodzących w skład agregatu powinny być wykonywane poprzez
/// korzeń agregatu, co zapewnia spójność danych w ramach agregatu.
/// <br />
/// Agregat jest jednym z podstawowych elementów modelu domenowego (DDD), który
/// pozwala na reprezentację bardziej złożonych struktur danych.
/// </remarks>
/// <typeparam name="TKey">
/// Typ identyfikatora.
/// </typeparam>
public interface IAggregate<TKey> : IAggregate, IEntity<TKey>
{
    
}