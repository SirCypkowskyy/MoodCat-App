using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.Notes;

namespace MoodCat.App.Core.Application.Notes.Commands.CreateNoteText;

/// <summary>
/// Komenda do stworzenia notatki
/// </summary>
/// <param name="RequestDataDTO">
/// Ciało żądania stworzenia notatki
/// </param>
/// <param name="UserId">
/// Id użytkownika, którego ma być notatka
/// </param>
public record CreateNoteTextCommand(CreateNoteRequestDTO RequestDataDTO, string UserId) : ICommand<CreateNoteTextResult>;

/// <summary>
/// Rezultat stworzenia notatki
/// </summary>
/// <param name="ResponseDataDTO"></param>
public record CreateNoteTextResult(CreateNoteResponseDTO ResponseDataDTO);