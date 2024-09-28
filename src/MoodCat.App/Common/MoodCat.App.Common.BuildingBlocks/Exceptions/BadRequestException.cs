namespace MoodCat.App.Common.BuildingBlocks.Exceptions;

/// <summary>
/// Wyjątek reprezentujący błąd zapytania
/// </summary>
public class BadRequestException : Exception
{
    /// <summary>
    /// Szczegóły błędu
    /// </summary>
    public string? Details { get; set; }
    
    /// <summary>
    /// Konstruktor wyjątku z domyślną wiadomością
    /// </summary>
    /// <param name="message">
    /// Wiadomość wyjątku
    /// </param>
    public BadRequestException(string message) : base(message)
    {
    }
    
    /// <summary>
    /// Konstruktor wyjątku z domyślną wiadomością i dodatkowymi szczegółami
    /// </summary>
    /// <param name="message">
    /// Wiadomość wyjątku
    /// </param>
    /// <param name="details">
    /// Szczegóły błędu
    /// </param>
    public BadRequestException(string message, string details) : base(message)
    {
        Details = details;
    }
}