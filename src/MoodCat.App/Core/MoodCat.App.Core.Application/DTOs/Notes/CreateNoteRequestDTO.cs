namespace MoodCat.App.Core.Application.DTOs.Notes;

/// <summary>
/// Obiekt transferu danych (DTO) do obsługi tworzenia nowej notatki (tekstowo lub dźwiękowo)
/// </summary>
/// <param name="Title">
/// Tytuł notatki
/// </param>
/// <param name="AudioUrl">
/// Ścieżka do pliku audio (to lub tekst)
/// </param>
/// <param name="Text">
/// Tekst notatki (to lub ścieżka url do nagrania)
/// </param>
/// <param name="Meta">
/// Meta stworzenia notatki
/// </param>
public record CreateNoteRequestDTO(
    string Title,
    string? AudioUrl,
    string? Text,
    CreateNoteMetaDTO Meta
);

/// <summary>
/// Obiekt transferu danych (DTO) mety tworzonej notatki
/// </summary>
/// <param name="ProvidedQuestion">
/// Opcjonalne, zadane pytanie przez chat
/// </param>
/// <param name="HappinessLevel">
/// Poziom zadowolenia z dnia (w skali 1-5)
/// </param>
public record CreateNoteMetaDTO(
    string? ProvidedQuestion,
    int HappinessLevel
);