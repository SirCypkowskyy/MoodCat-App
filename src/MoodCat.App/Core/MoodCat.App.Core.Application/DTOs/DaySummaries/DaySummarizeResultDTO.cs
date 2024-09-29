namespace MoodCat.App.Core.Application.DTOs.DaySummaries;

/// <summary>
/// Obiekt trasferu danych (DTO) dla podsumowania
/// </summary>
/// <param name="UserId">
/// Id użytkownika, do którego należy podsumowanie
/// </param>
/// <param name="Content">
/// Główna treść podsumowania
/// </param>
/// <param name="OriginalContent">
/// Główna treść podsumowania w oryginalnej wersji (niemodyfikowana przez użytkownika)
/// </param>
/// <param name="HappinessLevel">
/// Poziom zadowolenia (1-5, gdzie 1 to najgorszy możliwy stan, a 5 to stan idealny)
/// </param>
/// <param name="PatientGeneralFunctioning">
/// Podsumowanie na temat ogólnego funkcjonowania pacjenta
/// </param>
/// <param name="OriginalPatientGeneralFunctioning">
/// Podsumowanie na temat ogólnego funkcjonowania pacjenta (niemodyfikowane przez użytkownika)
/// </param>
/// <param name="Interests">
/// OPCJONALNY, informacje o nowych zainteresowaniach
/// </param>
/// <param name="OriginalInterests">
/// OPCJONALNY, informacje o nowych zainteresowaniach (niemodyfikowane przez użytkownika)
/// </param>
/// <param name="SocialRelationships">
/// OPCJONALNY, opis stanu relacji pacjenta, nowe znajomości, nowi wrogowie, nowe relacje romantyczne, zmiany w nich, etc.
/// </param>
/// <param name="OriginalSocialRelationships">
/// OPCJONALNY, opis stanu relacji pacjenta, nowe znajomości, nowi wrogowie, nowe relacje romantyczne, zmiany w nich, etc. (niezmodyfikowane przez użytkownika)
/// </param>
/// <param name="Work">
/// OPCJONALNY, opis zmian w życiu zawodowym
/// </param>
/// <param name="OriginalWork">
/// OPCJONALNY, opis zmian w życiu zawodowym (niezmodyfikowany przez użytkownika) 
/// </param>
/// <param name="Family">
/// OPCJONALNY, zmiany w życiu rodzinnym
/// </param>
/// <param name="OriginalFamily">
/// OPCJONALNY, zmiany w życiu rodzinnym (niezmodyfikowany przez użytkownika)
/// </param>
/// <param name="PhysicalHealth">
/// OPCJONALNY, zmiany w zdrowiu fizycznym, nowe choroby, poprawa zdrowia, etc.
/// </param>
/// <param name="OriginalPhysicalHealth">
/// OPCJONALNY, zmiany w zdrowiu fizycznym, nowe choroby, poprawa zdrowia, etc. (niezmodyfikowany przez użytkownika)
/// </param>
/// <param name="Memories">
/// OPCJONALNY, wspomnienia, o których myślał pacjent, jego refleksje na ich temat
/// </param>
/// <param name="OriginalMemories">
/// OPCJONALNY, wspomnienia, o których myślał pacjent, jego refleksje na ich temat (niezmodyfikowany przez użytkownika)
/// </param>
/// <param name="RepotedProblems">
/// OPCJONALNY, problemy napotkane przez pacjenta, nowe wyzwania, rozwiązanie wcześniej istniejących problemów
/// </param>
/// <param name="OriginalRepotedProblems">
/// OPCJONALNY, problemy napotkane przez pacjenta, nowe wyzwania, rozwiązanie wcześniej istniejących problemów  (niezmodyfikowany przez użytkownika)
/// </param>
/// <param name="Other">
/// OPCJONALNY, inne informacje, nie poruszane w innych kategoriach
/// </param>
/// <param name="OriginalOther">
/// OPCJONALNY, inne informacje, nie poruszane w innych kategoriach  (niezmodyfikowany przez użytkownika) 
/// </param>
public record DaySummarizeResultDTO(
    string UserId,
    string Content,
    string OriginalContent,
    decimal HappinessLevel,
    string? PatientGeneralFunctioning,
    string? OriginalPatientGeneralFunctioning,
    string? Interests,
    string? OriginalInterests,
    string? SocialRelationships,
    string? OriginalSocialRelationships,
    string? Work,
    string? OriginalWork,
    string? Family,
    string? OriginalFamily,
    string? PhysicalHealth,
    string? OriginalPhysicalHealth,
    string? Memories,
    string? OriginalMemories,
    string? RepotedProblems,
    string? OriginalRepotedProblems,
    string? Other,
    string? OriginalOther
);