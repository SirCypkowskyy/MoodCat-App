using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

namespace MoodCat.App.Core.Domain.Questions;

/// <summary>
/// Encja pytania
/// </summary>
public class QuestionEntity : Entity<long>
{
    /// <summary>
    /// Id usera-specialisty, który zlecił odpowiedzenie na pytanie
    /// </summary>
    public string SpecialistUserId { get; set; }
    
    /// <summary>
    /// Id usera, który musi odpowiedzieć na pytanie
    /// </summary>
    public string TargetUserId { get; set; }
    
    /// <summary>
    /// Id notatki, która jest odpowiedzią na notatkę
    /// </summary>
    public string? NoteAnswerId { get; set; }
    
    /// <summary>
    /// Pytanie specjalisty
    /// </summary>
    public string QuestionText { get; set; }
    
    /// <summary>
    /// Czy odpowiedź została odpowiedziana
    /// </summary>
    public bool IsAnswered => NoteAnswerId is not null && NoteAnswerId.Length > 0;
    
}