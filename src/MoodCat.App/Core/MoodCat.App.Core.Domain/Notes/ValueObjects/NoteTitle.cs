namespace MoodCat.App.Core.Domain.Notes.ValueObjects;

/// <summary>
/// Value object reprezentujący tytuł notatki.
/// </summary>
public record NoteTitle
{
    /// <summary>
    /// Maksymalna długość tytułu notatki.
    /// </summary>
    public const int DefaultMaxLength = 125;

    /// <summary>
    /// Wartość tytułu notatki.
    /// </summary>
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// Tworzy nowy tytuł notatki.
    /// </summary>
    /// <param name="value">
    /// Wartość tytułu notatki.
    /// </param>
    private NoteTitle(string value) => Value = value;

    /// <summary>
    /// Tworzy nowy tytuł notatki.
    /// </summary>
    /// <param name="value">
    /// Wartość tytułu notatki.
    /// </param>
    /// <returns>
    /// Nowy tytuł notatki.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Wyrzucany, gdy długość tytułu notatki przekracza <see cref="DefaultMaxLength"/> znaków.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Wyrzucany, gdy wartość tytułu notatki jest <see langword="null"/>.
    /// </exception>
    public static NoteTitle Of(string value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        if (value.Length > DefaultMaxLength)
            throw new ArgumentException($"Długość tytułu notatki nie może przekraczać {DefaultMaxLength} znaków.",
                nameof(value));

        return new NoteTitle(value);
    }
}