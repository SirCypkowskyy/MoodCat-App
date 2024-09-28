namespace MoodCat.App.Core.Application.DTOs.Notes;

/// <summary>
/// Notataka stworzona przez Chat
/// </summary>
/// <param name="NoteId">
/// Id notatki
/// </param>
/// <param name="Title">
/// Tytuł notatki
/// </param>
/// <param name="Content">
/// Zawartość stworzona przez Chat
/// </param>
public record CreateNoteResponseDTO(
    string NoteId,
    string Title,
    string Content
);