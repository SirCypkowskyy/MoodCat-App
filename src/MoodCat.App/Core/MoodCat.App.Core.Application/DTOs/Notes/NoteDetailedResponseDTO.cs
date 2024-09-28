using MoodCat.App.Core.Domain.Notes;

namespace MoodCat.App.Core.Application.DTOs.Notes;

/// <summary>
/// Zaawansowany obiekt transferu danych (DTO) dla notatki
/// </summary>
/// <param name="Id">
/// Id notatki
/// </param>
/// <param name="Content">
/// Zawartość notatki
/// </param>
/// <param name="Title">
/// Tytuł notatki
/// </param>
/// <param name="CreatedAt">
/// Data stworzenia notatki
/// </param>
/// <param name="UpdatedAt">
/// Data ostatniej aktualizacji notatki (opcjonalne pole)
/// </param>
/// <param name="Happiness">
/// Poziom zadowolenia zaznaczony w notatce
/// </param>
/// <param name="Attachments">
/// Załączniki do notatki
/// </param>
public record NoteDetailedResponseDTO(
    string Id,
    string Content,
    string Title,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    double Happiness,
    NoteAttachmentDetailedResponseDTO[] Attachments
);

/// <summary>
/// Zaawansowane obiekt transferu danych dla załącznika notatki
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="ResourceUrl"></param>
/// <param name="CreatedAt"></param>
/// <param name="UpdatedAt"></param>
public record NoteAttachmentDetailedResponseDTO(
    string Id,
    string Name,
    string ResourceUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);