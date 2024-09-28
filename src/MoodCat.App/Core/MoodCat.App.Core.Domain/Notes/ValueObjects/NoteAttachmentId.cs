namespace MoodCat.App.Core.Domain.Notes.ValueObjects;

/// <summary>
/// Value object reprezentujący identyfikator załącznika notatki.
/// </summary>
public record NoteAttachmentId
{
    /// <summary>
    /// Wartość identyfikatora.
    /// </summary>
    public Guid Value { get; init; }

    /// <summary>
    /// Tworzy nowy identyfikator załącznika notatki.
    /// </summary>
    /// <param name="value">
    /// Wartość identyfikatora.
    /// </param>
    private NoteAttachmentId(Guid value) => Value = value;

    /// <summary>
    /// Tworzy nowy identyfikator załącznika notatki.
    /// </summary>
    /// <param name="value">
    /// Wartość identyfikatora.
    /// </param>
    /// <returns>
    /// Nowy identyfikator załącznika notatki.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Wyrzucany, gdy wartość identyfikatora jest pusta.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Wyrzucany, gdy wartość identyfikatora jest <see langword="null"/>.
    /// </exception>
    public static NoteAttachmentId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        if (value == Guid.Empty)
            throw new ArgumentException("Wartość identyfikatora nie może być pusta.", nameof(value));

        return new NoteAttachmentId(value);
    }
}