using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    /// <remarks>
    /// Może mieć wartość 0 - wtedy oznacza to, że żadna notatka nie miała ustawionego poziomu
    /// zadowolenia z dnia
    /// </remarks>
    public double HappinessLevel => _notes
        .Where(s => s.Happiness is not null && s.Happiness >= 0)
        .Average(s => s.Happiness) ?? 0;
    
    /// <summary>
    /// Podsumowanie na temat ogólnego funkcjonowania pacjenta
    /// </summary>
    public string? PatientGeneralFunctioning { get; private set; }
    
    /// <summary>
    /// Podsumowanie na temat ogólnego funkcjonowania pacjenta
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalPatientGeneralFunctioning { get; private set; }
    
    /// <summary>
    /// Podsumowanie o nowych zainteresowaniach / zmianach w nich
    /// </summary>
    public string? Interests { get; private set; }
    
    /// <summary>
    /// Podsumowanie o nowych zainteresowaniach / zmianach w nich
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalInterests { get; private set; }
    
    /// <summary>
    /// Podsumowanie o stanie relacji pacjenta, nowych znajomościah,
    /// nowych wrogach, nowych relacjach romantycznych, zmianach w nich, etc.
    /// </summary>
    public string? SocialRelationships { get; private set; }

    /// <summary>
    /// Podsumowanie o stanie relacji pacjenta, nowych znajomościah,
    /// nowych wrogach, nowych relacjach romantycznych, zmianach w nich, etc.
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalSocialRelationships { get; private set; }
    
    /// <summary>
    /// Podsumowanie-opis zmian w życiu zawodowym
    /// </summary>
    public string? Work { get; private set; }
    
    /// <summary>
    /// Podsumowanie-opis zmian w życiu zawodowym
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalWork { get; private set; }
    
    /// <summary>
    /// Podsumowanie zmian w życiu prywatnym/rodzinnym
    /// </summary>
    public string? Family  { get; private set; }
    
    /// <summary>
    /// Podsumowanie zmian w życiu prywatnym/rodzinnym
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalFamily { get; private set; }
    
    /// <summary>
    /// Podsumowanie stanu fizycznego pacjenta (choroby, poprawa zdrowia, kondycji, etc.)
    /// </summary>
    public string? PhysicalHealth { get; private set; }
    
    /// <summary>
    /// Podsumowanie stanu fizycznego pacjenta (choroby, poprawa zdrowia, kondycji, etc.)
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalPhysicalHealth { get; private set; }
    
    /// <summary>
    /// Podsumowanie wspomnień, o których myślał pacjent, jego refleksje na ich temat
    /// </summary>
    public string? Memories { get; private set; }
    
    /// <summary>
    /// Podsumowanie wspomnień, o których myślał pacjent, jego refleksje na ich temat
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalMemories { get; private set; }
    
    /// <summary>
    /// Podsumowane problemy napotkane przez pacjenta, nowe wyzwania,
    /// rozwiązanie wcześniej istniejących problemów
    /// </summary>
    public string? ReportedProblems { get; private set; }
    
    /// <summary>
    /// Podsumowane problemy napotkane przez pacjenta, nowe wyzwania,
    /// rozwiązanie wcześniej istniejących problemów
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalReportedProblems { get; private set; }
    
    /// <summary>
    /// Podsumowane inne informacje, nie poruszane w innych kategoriach
    /// </summary>
    public string? Other { get; private set; }
    
    /// <summary>
    /// Podsumowane inne informacje, nie poruszane w innych kategoriach
    /// </summary>
    /// <remarks>
    /// Oryginalna rubryka (danych stworzonych przez AI)
    /// </remarks>
    public string? OriginalOther { get; private set; }
    
    
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

    /// <summary>
    /// Podsumowanie dnia
    /// </summary>
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
            SummaryDate = DateOnly.FromDateTime(DateTime.Now),
        };
        
        return daySummary;
    }

    /// <summary>
    /// Tworzy podsumowanie dnia dla wybranego usera, starając się uzupełnić wybrane kolumny
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="content"></param>
    /// <param name="patientGeneralFunctioning"></param>
    /// <param name="interests"></param>
    /// <param name="socialRelationships"></param>
    /// <param name="work"></param>
    /// <param name="family"></param>
    /// <param name="physicalHealth"></param>
    /// <param name="memories"></param>
    /// <param name="reportedProblems"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static DaySummaryEntity Create(string userId, string content, string? patientGeneralFunctioning,
        string? interests, string? socialRelationships, string? work,
        string? family, string? physicalHealth, string? memories, string? reportedProblems,
        string? other)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);
        ArgumentException.ThrowIfNullOrEmpty(content);

        var daySummary = new DaySummaryEntity()
        {
            UserId = userId,
            Content = content,
            OriginalContent = content,
            SummaryDate = DateOnly.FromDateTime(DateTime.Now),
            PatientGeneralFunctioning = patientGeneralFunctioning,
            OriginalPatientGeneralFunctioning = patientGeneralFunctioning,
            Interests = interests,
            OriginalInterests = interests,
            SocialRelationships = socialRelationships,
            OriginalSocialRelationships = socialRelationships,
            Work = work,
            OriginalWork = work,
            Family = family,
            OriginalFamily = family,
            PhysicalHealth = physicalHealth,
            OriginalPhysicalHealth = physicalHealth,
            Memories = memories,
            OriginalMemories = memories,
            ReportedProblems = reportedProblems,
            OriginalReportedProblems = reportedProblems,
            Other = other,
            OriginalOther = other
        };
        
        return daySummary;
    }

    /// <summary>
    /// Aktualizuje content podsumowania dnia
    /// </summary>
    /// <param name="content">
    /// Nowy kontent podsumowania dnia
    /// </param>
    public void UpdateContent(string content)
    {
        ArgumentNullException.ThrowIfNull(content);
        Content = content;
    }

    public void UpdatePatientGeneralFunctioning(string patientGeneralFunctioning)
    => PatientGeneralFunctioning = patientGeneralFunctioning;
    
    public void UpdateInterests(string interests)
    => Interests = interests;
    
    public void UpdateSocialRelationships(string socialRelationships)
    => SocialRelationships = socialRelationships;
    
    public void UpdateWork(string work)
    => Work = work;
    
    public void UpdateFamily(string family) 
    => Family = family;
    
    public void UpdatePhysicalHealth(string physicalHealth)
    => PhysicalHealth = physicalHealth;
    
    public void UpdateMemories(string memories)
    => Memories = memories;
    
    public void UpdateReportedProblems(string reportedProblems)
    => ReportedProblems = reportedProblems;
    
    public void UpdateOther(string other)
     => Other = other;
}