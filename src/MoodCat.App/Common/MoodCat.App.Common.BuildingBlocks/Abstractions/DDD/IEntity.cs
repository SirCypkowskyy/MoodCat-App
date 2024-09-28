namespace MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

/// <summary>
/// Interfejs reprezentujący encję (w ramach wzorca DDD).
/// </summary>
/// <remarks>
/// Encja jest podstawowym elementem modelu domenowego. Z jej pomocą można
/// reprezentować obiekty, które mają swoje własne życie i stan. Encja
/// charakteryzuje się tym, że posiada identyfikator, który pozwala na
/// jednoznaczną identyfikację obiektu w ramach systemu.
/// </remarks>
public interface IEntity : IAuditableEntity
{
    
}

/// <summary>
/// Interfejs reprezentujący encję (w ramach wzorca DDD) z identyfikatorem.
/// </summary>
/// <remarks>
/// Encja jest podstawowym elementem modelu domenowego. Z jej pomocą można
/// reprezentować obiekty, które mają swoje własne życie i stan. Encja
/// charakteryzuje się tym, że posiada identyfikator, który pozwala na
/// jednoznaczną identyfikację obiektu w ramach systemu.
/// </remarks>
/// <typeparam name="TKey">
/// Typ identyfikatora.
/// </typeparam>
public interface IEntity<TKey> : IEntity
{
    /// <summary>
    /// Identyfikator encji.
    /// </summary>
    /// <remarks>
    /// Identyfikator encji pozwala na jednoznaczną identyfikację obiektu
    /// w ramach systemu. Identyfikator jest unikalny w ramach systemu i
    /// nie może być zmieniany po utworzeniu encji.
    /// </remarks>
    TKey Id { get; set;  }
}