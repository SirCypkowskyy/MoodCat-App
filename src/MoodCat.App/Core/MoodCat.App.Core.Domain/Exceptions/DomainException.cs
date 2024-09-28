namespace MoodCat.App.Core.Domain.Exceptions;

/// <summary>
/// Wyjątek domenowy, rzucany dla błędów związanych z logiką biznesową.
/// </summary>
/// <remarks>
/// Wyjątek ten powinien być rzucany w warstwie domenowej, gdy logika biznesowa nie może zostać wykonana.
/// Dzięki zastosowaniu tego wyjątku, możliwe jest łatwe rozróżnienie błędów domenowych (związanych z logiką biznesową)
/// od błędów technicznych (związanych z infrastrukturą, np. bazą danych).
/// </remarks>
public class DomainException : Exception
{
    /// <summary>
    /// Konstruktor wyjątku domenowego.
    /// </summary>
    /// <param name="message">
    /// Wiadomość wyjątku.
    /// </param>
    public DomainException(string message) : base($"Domain exception: {message} thrown from Domain layer")
    {
    }
}