namespace MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

/// <summary>
/// Interfejs reprezentujący encję, która posiada informacje o audycji (dane o autorze, dacie utworzenia, dacie modyfikacji, etc.).
/// </summary>
/// <remarks>
/// Z pomocą tego interfejsu można oznaczyć encję, która posiada informacje o audycji.
/// Encje oznaczone tym interfejsem będą automatycznie wzbogacane o informacje o autorze, dacie utworzenia, dacie modyfikacji, etc.
/// Jest to przydatne w przypadku, gdy chcemy śledzić kto i kiedy utworzył lub zmodyfikował encję.
/// </remarks>
public interface IAuditableEntity
{
    /// <summary>
    /// Data utworzenia encji.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
    
    /// <summary>
    /// Autor, który utworzył encję.
    /// </summary>
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// Data ostatniej modyfikacji encji.
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }
    
    /// <summary>
    /// Autor, który ostatnio zmodyfikował encję.
    /// </summary>
    public string? LastModifiedBy { get; set; }
}