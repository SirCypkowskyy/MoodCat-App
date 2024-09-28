using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;
using MoodCat.App.Core.Domain.Notes.ValueObjects;

namespace MoodCat.App.Core.Domain.Notes;

/// <summary>
/// Encja załącznika notatki.
/// </summary>
public class NoteAttachment : Entity<NoteAttachmentId>
{
    /// <summary>
    /// Maksymalna nazwa załącznika
    /// </summary>
    public const int MaxNameLength = 128;

    /// <summary>
    /// Nazwa załącznika.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Rozmiar załącznika.
    /// </summary>
    public long Size { get; init; }

    /// <summary>
    /// Maksymalna długość url do załącznika
    /// </summary>
    public const int MaxResourceUrlLength = 500;
    
    /// <summary>
    /// URL zasobu załącznika.
    /// </summary>
    public string ResourceUrl { get; init; } = string.Empty;

    /// <summary>
    /// Tworzy nowy załącznik notatki.
    /// </summary>
    /// <remarks>
    /// Publiczny, pusty konstruktor jest wymagany przez zewnętrzne biblioteki do deserializacji obiektów.
    /// </remarks>
    public NoteAttachment()
    {
    }

    /// <summary>
    /// Tworzy nowy załącznik notatki.
    /// </summary>
    /// <param name="name">
    /// Nazwa załącznika.
    /// </param>
    /// <param name="size">
    /// Rozmiar załącznika.
    /// </param>
    /// <param name="resourceUrl">          
    /// URL zasobu załącznika.
    /// </param>
    private NoteAttachment(string name, long size, string resourceUrl)
    {
        Id = NoteAttachmentId.Of(Guid.NewGuid());
        Name = name;
        Size = size;
        ResourceUrl = resourceUrl;
    }

    /// <summary>
    /// Tworzy nowy załącznik notatki.
    /// </summary>
    /// <param name="name">
    /// Nazwa załącznika.
    /// </param>
    /// <param name="size">
    /// Rozmiar załącznika.
    /// </param>
    /// <param name="resourceUrl">
    /// URL zasobu załącznika.
    /// </param>
    /// <returns>
    /// Nowy załącznik notatki.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Wyrzucany, gdy nazwa załącznika jest pusta.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Wyrzucany, gdy nazwa załącznika jest <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Wyrzucany, gdy rozmiar załącznika jest mniejszy od zera.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Wyrzucany, gdy URL zasobu załącznika jest <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Wyrzucany, gdy URL zasobu załącznika jest pusty.
    /// </exception>
    public static NoteAttachment Of(string name, long size, string resourceUrl)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(resourceUrl, nameof(resourceUrl));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nazwa załącznika nie może być pusta.", nameof(name));
        if (size < 0)
            throw new ArgumentException("Rozmiar załącznika nie może być mniejszy od zera.", nameof(size));
        if (string.IsNullOrWhiteSpace(resourceUrl))
            throw new ArgumentException("URL zasobu załącznika nie może być pusty.", nameof(resourceUrl));

        if (name.Length > MaxNameLength)
            throw new ArgumentException($"Nazwa załącznika nie może przekraczać {MaxNameLength} znaków!");

        if (resourceUrl.Length > MaxResourceUrlLength)
            throw new ArgumentException($"Url do załącznika nie może przekraczać {MaxResourceUrlLength} znaków!");
        
        
        return new NoteAttachment(name, size, resourceUrl);
    }
}