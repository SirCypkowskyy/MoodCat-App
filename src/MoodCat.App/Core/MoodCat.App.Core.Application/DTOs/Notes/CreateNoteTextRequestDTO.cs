namespace MoodCat.App.Core.Application.DTOs.Notes;

/// <summary>
/// Obiekt transferu danych (DTO) do obsługi tworzenia nowej notatki (tekstowo)
/// </summary>
/// <param name="Title">
/// Tytuł notatki
/// </param>
/// <param name="Body">
/// Ciało notatki
/// </param>
/// <param name="HappinessLevel">
/// Poziom zadowolenia z dnia
/// </param>
public record CreateNoteTextRequestDTO(
    string Title,
    string Body,
    int HappinessLevel
);