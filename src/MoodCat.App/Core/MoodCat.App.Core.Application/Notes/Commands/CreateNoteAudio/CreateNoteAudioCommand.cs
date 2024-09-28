using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.Notes;

namespace MoodCat.App.Core.Application.Notes.Commands.CreateNoteAudio;

/// <summary>
/// Tworzy nową notatkę z audio
/// </summary>
/// <param name="NoteTitle">
/// Tytuł notatki
/// </param>
/// <param name="AudioUrl">
/// Adres do pliku audio
/// </param>
/// <param name="UserId">
/// Id użytkownika
/// </param>
/// <param name="HappinessScore">
/// Poziom zadowolenia 
/// </param>
public record CreateNoteAudioCommand(
    string NoteTitle,
    string AudioUrl,
    string UserId,
    int HappinessScore,
    string? ProvidedQuestion
) : ICommand<CreateNoteAudioResult>;

/// <summary>
/// Wynik operacji stworzenia notatki z pliku audio
/// </summary>
/// <param name="ResponseDataDTO">Ciało odpowiedzi</param>
public record CreateNoteAudioResult(
    CreateNoteResponseDTO ResponseDataDTO
);