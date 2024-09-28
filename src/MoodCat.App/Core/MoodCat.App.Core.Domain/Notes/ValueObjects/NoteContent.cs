namespace MoodCat.App.Core.Domain.Notes.ValueObjects;


/// <summary>
/// Value object reprezentujący zawartość notatki.
/// </summary>
public record NoteContent
{
    /// <summary>
    /// Domyślna, maksymalna długość zawartości notatki.
    /// </summary>
    public const int DefaultMaxLength = 10_000;
    
    /// <summary>
    /// Wartość zawartości notatki.
    /// </summary>
    public string Value { get; init; } = string.Empty;
    
    /// <summary>
    /// Tworzy nową zawartość notatki.
    /// </summary>
    /// <param name="value">
    /// Wartość zawartości notatki.
    /// </param>
    private NoteContent(string value) => Value = value;
    
    /// <summary>
    /// Tworzy nową zawartość notatki.
    /// </summary>
    /// <param name="value">
    /// Wartość zawartości notatki.
    /// </param>
    /// <returns>
    /// Nowa zawartość notatki.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Wyrzucany, gdy wartość zawartości notatki przekracza domyślną maksymalną długość.
    /// </exception>
    public static NoteContent Of(string value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        if(value.Length > DefaultMaxLength)
            throw new ArgumentException($"Zawartość notatki nie może przekraczać {DefaultMaxLength} znaków.", nameof(value));
        
        return new NoteContent(value);
    }
}