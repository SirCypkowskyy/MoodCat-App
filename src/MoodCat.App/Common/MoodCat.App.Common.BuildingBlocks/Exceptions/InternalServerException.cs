namespace MoodCat.App.Common.BuildingBlocks.Exceptions;

/// <summary>
/// Wyjątek dotyczący błędu serwera
/// </summary>
public class InternalServerException : Exception
{
    /// <summary>
    /// Konstruktor klasy <see cref="InternalServerException"/> 
    /// </summary>
    /// <param name="message">
    /// Wiadomość błędu
    /// </param>
    public InternalServerException(string message) : base(message)
    {
    }
    
    /// <summary>
    /// Konstruktor klasy <see cref="InternalServerException"/> z możliwością przekazania wyjątku wewnętrznego
    /// </summary>
    /// <param name="message">
    /// Wiadomość błędu
    /// </param>
    /// <param name="innerException">
    /// Wyjątek wewnętrzny
    /// </param>
    public InternalServerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}