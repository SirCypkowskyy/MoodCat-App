using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;
using MoodCat.App.Core.Domain.DaySummaries;
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
    /// Klucz opcy opcjonalnego podsumowania dnia, do którego notatka należy 
    /// </summary>
    public Guid? DaySummaryId { get; private set; }

    /// <summary>
    /// Poziom zadowolenia z dnia
    /// </summary>
    public double? Happiness { get; private set; }

    // /// <summary>
    // /// Id pytania, na którego notatka jest odpowiedzią
    // /// </summary>
    // public long? QuestionId { get; private set; }
    //
    // /// <summary>
    // /// Czy notatka jest odpowiedzią na pytanie
    // /// </summary>
    // public bool IsAnswerToQuestion => QuestionId is not null;

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
    /// <param name="happinessLevel">
    /// (opcjonalny) Poziom zadowolenia z dnia
    /// </param>
    /// <returns></returns>
    public static NoteEntity Create(string userId, NoteTitle title, NoteContent content, int? happinessLevel)
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
            Happiness = happinessLevel
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
    /// Ustawia suprevisora notatki
    /// </summary>
    /// <param name="userId"></param>
    public void SetAllowedNoteSupervisorId(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId, nameof(userId));
        AllowedNoteSupervisorId = userId;
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

    public void UpdateDaySummaryId(Guid? daySummaryId)
    {
        ArgumentNullException.ThrowIfNull(daySummaryId, nameof(daySummaryId));
        DaySummaryId = daySummaryId;
    }

    // public void AssignToQuestion(long questionId)
    // {
    //     ArgumentNullException.ThrowIfNull(questionId, nameof(questionId));
    //
    //     QuestionId = questionId;
    // }
}