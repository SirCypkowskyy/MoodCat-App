using System.ComponentModel.DataAnnotations;
using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;
using MoodCat.App.Core.Domain.Notes;

namespace MoodCat.App.Core.Domain.DaySummaries;

/// <summary>
/// Encja dla podsumowania dnia
/// </summary>
public class DaySummaryEntity : Aggregate<Guid>
{
    /// <summary>
    /// Data podsumowania
    /// </summary>
    public DateOnly SummaryDate { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
    
    /// <summary>
    /// Id użytkownika, dla którego powstało summary
    /// </summary>
    public string UserId { get; private set; }
    
    /// <summary>
    /// Najnowszy kontent podsumowania dnia
    /// </summary>
    public string Content { get; private set; }
    
    /// <summary>
    /// Oryginalny kontent podsumowania dnia (wygenrowany przez AI)
    /// </summary>
    public string OriginalContent { get; private set; }
    
    /// <summary>
    /// Poziom zadowolenia z dnia
    /// </summary>
    [Range(1, 5)]
    public int HappinessLevel { get; private set; }
    
    /// <summary>
    /// Notatki, które zostały użyte do stworzenia podsumowania
    /// </summary>
    private readonly List<NoteEntity> _notes = [];
    
    /// <summary>
    /// Notatki, które zostały użyte do stworzenia podsumowania
    /// </summary>
    public IReadOnlyList<NoteEntity> Notes => _notes.AsReadOnly();

    private DaySummaryEntity(string userId, string content, string originalContent)
    {
        UserId = userId;
        Content = content;
        OriginalContent = originalContent;
    }

    public DaySummaryEntity() {}
    
    /// <summary>
    /// Tworzy nowe podsumowanie dnia
    /// </summary>
    /// <param name="daySummaryId"></param>
    /// <param name="userId"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static DaySummaryEntity Create(string userId, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);
        ArgumentException.ThrowIfNullOrEmpty(content);

        var daySummary = new DaySummaryEntity()
        {
            UserId = userId,
            Content = content,
            OriginalContent = content,
        };
        
        return daySummary;
    }           

}