using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.Notes;

namespace MoodCat.App.Core.Application.Notes.Queries.GetNoteById;

/// <summary>
/// Zapytanie pobierania szczegółowej notatki po id
/// </summary>
/// <param name="NoteId">
/// Id notatki
/// </param>
public record GetNoteByIdQuery(Guid NoteId, string UserId) : IQuery<GetNoteByIdResult>;

/// <summary>
/// Rezultat prawidłowego uruchomienia zapytania pobierania szczegółowej notatki po id
/// </summary>
/// <param name="Data">
/// Dane notatki
/// </param>
public record GetNoteByIdResult(NoteDetailedResponseDTO Data);