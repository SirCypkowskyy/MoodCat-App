namespace MoodCat.App.Core.Domain.Users.ValueObjects;

/// <summary>
/// Typ identyfikatora encji <see cref="User"/>.
/// </summary>
public record UserId
{
    /// <summary>
    /// Wartość identyfikatora.
    /// </summary>
    public Guid Value { get; init; }

    /// <summary>
    /// Tworzy nowy identyfikator encji <see cref="User"/>.
    /// </summary>
    /// <param name="value">
    /// Wartość identyfikatora.
    /// </param>
    private UserId(Guid value) => Value = value;

    /// <summary>
    /// Tworzy nowy identyfikator encji <see cref="User"/>.
    /// </summary>
    /// <param name="value">
    /// Wartość identyfikatora.
    /// </param>
    /// <returns>
    /// Nowy identyfikator encji <see cref="User"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Wyrzucany, gdy wartość identyfikatora jest pusta.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Wyrzucany, gdy wartość identyfikatora jest <see langword="null"/>.
    /// </exception>
    public static UserId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        if (value == Guid.Empty)
            throw new ArgumentException("Wartość identyfikatora nie może być pusta.", nameof(value));

        return new UserId(value);
    }
}   