using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;
using MoodCat.App.Core.Domain.Notes.Events;
using MoodCat.App.Core.Domain.Notes.ValueObjects;

namespace MoodCat.App.Core.Domain.Notes;


/// <summary>
/// Encja reprezentująca notatkę.
/// </summary>
public class NoteEntity : Aggregate<NoteId>
{
    /// <summary>
    /// Identyfikator użytkownika, do którego należy notatka.
    /// </summary>
    public string UserId { get; private set; } = default!;
    
    /// <summary>
    /// Opcjonalny identyfikator użytkownika, który jest
    /// dozwolony do wyświetlania notatki jako doktor/terapeuta 
    /// </summary>
    public string? AllowedNoteSupervisorId { get; private set; }
    
    /// <summary>
    /// Tytuł notatki.
    /// </summary>
    public NoteTitle Title { get; private set; } = default!;
    
    /// <summary>
    /// Zawartość notatki.
    /// </summary>
    public NoteContent Content { get; private set; } = default!;
    
    /// <summary>
    /// Załączniki notatki.
    /// </summary>
    private readonly List<NoteAttachment> _attachments = [];
    
    /// <summary>
    /// Załączniki notatki.
    /// </summary>
    public IReadOnlyList<NoteAttachment> Attachments => _attachments.AsReadOnly();

    /// <summary>
    /// Tworzy nową notatkę.
    /// </summary>
    /// <param name="userId">
    /// Id twórcy notatki
    /// </param>
    /// <param name="title">
    /// Tytuł notakti
    /// </param>
    /// <param name="content">
    /// Zawartość notatki
    /// </param>
    /// <returns></returns>
    public static NoteEntity Create(string userId, NoteTitle title, NoteContent content)
    {
        ArgumentNullException.ThrowIfNull(userId, nameof(userId));
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        ArgumentNullException.ThrowIfNull(content, nameof(content));
        
        var note = new NoteEntity
        {
            Id = NoteId.Of(Guid.NewGuid()),
            UserId = userId,    
            Title = title,
            Content = content,
        };
        
        note.AddDomainEvent(new NoteCreatedDomainEvent(note));

        return note;
    }
    
    /// <summary>
    /// Aktualizuje notatkę.
    /// </summary>
    /// <param name="title">
    /// Nowy tytuł notatki.
    /// </param>
    /// <param name="content">
    /// Nowa zawartość notatki.
    /// </param>
    public void Update(NoteTitle title, NoteContent content)
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        ArgumentNullException.ThrowIfNull(content, nameof(content));
        
        Title = title;
        Content = content;
        
        AddDomainEvent(new NoteUpdatedDomainEvent(this));
    }
    
    /// <summary>
    /// Dodaje załącznik do notatki.
    /// </summary>
    /// <param name="attachment">
    /// Załącznik do dodania.
    /// </param>
    public void AddAttachment(NoteAttachment attachment)
    {
        ArgumentNullException.ThrowIfNull(attachment, nameof(attachment));
        
        _attachments.Add(attachment);
    }
    
    /// <summary>
    /// Usuwa załącznik z notatki.
    /// </summary>
    /// <param name="attachment">
    /// Załącznik do usunięcia.
    /// </param>
    public void RemoveAttachment(NoteAttachment attachment)
    {
        ArgumentNullException.ThrowIfNull(attachment, nameof(attachment));
        
        _attachments.Remove(attachment);
    }
}