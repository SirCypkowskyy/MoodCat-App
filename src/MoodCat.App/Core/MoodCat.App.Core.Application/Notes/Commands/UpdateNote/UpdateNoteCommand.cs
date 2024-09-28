using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.Notes;

namespace MoodCat.App.Core.Application.Notes.Commands.UpdateNote;

/// <summary>
/// Komenda do aktualizacji notatki
/// </summary>
/// <param name="NoteId">
/// Id notatki do aktualizacji
/// </param>
/// <param name="Data">
/// Dane notatki do aktualizacji
/// </param>
/// <param name="UserId">
/// Id użytkownika
/// </param>
public record UpdateNoteCommand(Guid NoteId, CreateNoteRequestDTO Data, string UserId) : ICommand<UpdateNoteResult>;

/// <summary>
/// Rezultat wykonania komendy aktualizacji notatki
/// </summary>
/// <param name="ResponseDataDTO">
/// Dane zaktualizowanej notatki
/// </param>
public record UpdateNoteResult(CreateNoteResponseDTO ResponseDataDTO);