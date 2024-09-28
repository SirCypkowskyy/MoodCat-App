namespace MoodCat.App.Core.Domain.Notes.ValueObjects;

/// <summary>
/// Typ identyfikatora encji <see cref="Note"/>.
/// </summary>
public record NoteId
{
    /// <summary>
    /// Wartość identyfikatora.
    /// </summary>
    public Guid Value { get; init; }
    
    private NoteId(Guid value) => Value = value;

    /// <summary>
    /// Tworzy nowy identyfikator encji <see cref="Note"/>.
    /// </summary>
    /// <param name="value">
    /// Wartość identyfikatora.
    /// </param>
    /// <returns>
    /// Nowy identyfikator encji <see cref="Note"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Wyrzucany, gdy wartość identyfikatora jest pusta.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Wyrzucany, gdy wartość identyfikatora jest <see langword="null"/>.
    /// </exception>
    public static NoteId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        if(value == Guid.Empty)
            throw new ArgumentException("Wartość identyfikatora nie może być pusta.", nameof(value));
        
        return new NoteId(value);
    }
}