using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.Notes;

namespace MoodCat.App.Core.Application.Notes.Queries.GetTodaysNotes;

/// <summary>
/// Żądanie pobrania wszystkich notatek użytkownika z dzisiejszego dnia
/// </summary>
/// <param name="UserId">
/// Id użytkownika
/// </param>
public record GetTodaysNotesQuery(
    string UserId
) : IQuery<GetTodaysNotesResult>;

/// <summary>
/// Wynik żądania pobrania wszystkich notatek użytkownika z dzisiejszego dnia
/// </summary>
/// <param name="Notes">
/// Notatki
/// </param>
public record GetTodaysNotesResult(
    NoteDetailedResponseDTO[] Notes
);