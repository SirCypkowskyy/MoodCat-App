namespace MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

/// <summary>
/// Abstrakcyjna klasa bazowa reprezentująca encję (w ramach wzorca DDD).
/// </summary>
/// <remarks>
/// Encja jest podstawowym elementem modelu domenowego. Z jej pomocą można
/// reprezentować obiekty, które mają swoje własne życie i stan. Encja
/// charakteryzuje się tym, że posiada identyfikator, który pozwala na
/// jednoznaczną identyfikację obiektu w ramach systemu.
/// <br />
/// Klasa bazowa posiada pola reprezentujące informacje o autorze i dacie
/// utworzenia oraz o autorze i dacie ostatniej modyfikacji encji. Dzięki
/// tym informacjom możliwe jest śledzenie historii zmian encji w systemie.
/// </remarks>
/// <typeparam name="TKey">
/// Typ identyfikatora.
/// </typeparam>
public abstract class Entity<TKey> : IEntity<TKey>
{
    /// <inheritdoc />
    public TKey Id { get; set; }
    
    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }
    
    /// <inheritdoc />
    public string? CreatedBy { get; set; }
    
    /// <inheritdoc />
    public DateTime? LastModifiedAt { get; set; }
    
    /// <inheritdoc />
    public string? LastModifiedBy { get; set; }
}
