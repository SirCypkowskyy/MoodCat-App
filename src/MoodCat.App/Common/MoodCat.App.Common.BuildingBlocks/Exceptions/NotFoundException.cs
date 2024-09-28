namespace MoodCat.App.Common.BuildingBlocks.Exceptions;

/// <summary>
/// Wyjątek dotyczący błędu serwera
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Konstruktor klasy <see cref="NotFoundException"/>
    /// </summary>
    /// <param name="message">
    /// Wiadomość błędu
    /// </param>
    public NotFoundException(string message) : base(message)
    {
    }
    
    
    /// <summary>
    /// Konstruktor klasy <see cref="NotFoundException"/> z nazwą nie znalezionej encji i kluczem
    /// </summary>
    /// <param name="name">
    /// Nazwa encji, która nie została znaleziona
    /// </param>
    /// <param name="key">
    /// Klucz encji, która nie została znaleziona
    /// </param>
    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}