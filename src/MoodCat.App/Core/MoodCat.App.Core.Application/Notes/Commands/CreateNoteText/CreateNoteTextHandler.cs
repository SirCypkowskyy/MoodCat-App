using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.Notes;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.Notes.Commands.CreateNoteText;

/// <summary>
/// Handler obsługi żądania stworzenia notatki
/// </summary>
public class CreateNoteTextHandler(IApplicationDbContext dbContext, ILogger<CreateNoteTextHandler> logger)
    : ICommandHandler<CreateNoteTextCommand, CreateNoteTextResult>
{
    /// <inheritdoc />
    public async Task<CreateNoteTextResult> Handle(CreateNoteTextCommand command, CancellationToken cancellationToken)
    {
        // Sprawdź, czy użytkownik z wybranym kluczem istnieje
        if (!dbContext.Users.Any(x => x.Id == command.UserId))
            throw new ArgumentException("Nie znaleziono użytkownika z takim ID!");

        string noteContent;

        if (!string.IsNullOrWhiteSpace(command.RequestDataDTO.Meta.ProvidedQuestion))
            noteContent = "Question: " + command.RequestDataDTO.Meta.ProvidedQuestion + "\n\n" + "Answer: " +
                          command.RequestDataDTO.Text;
        else
            noteContent = command.RequestDataDTO.Text ?? "No note content";

        var note = NoteEntity.Create(
            command.UserId,
            NoteTitle.Of(command.RequestDataDTO.Title),
            NoteContent.Of(noteContent),
            command.RequestDataDTO.Meta.HappinessLevel
        );

        var usr = (await dbContext.Users.FindAsync(new object?[] { command.UserId }, cancellationToken))!;

        if (usr.AssignedSpecialistId != null)
            note.SetAllowedNoteSupervisorId(usr.AssignedSpecialistId);

        await dbContext.Notes.AddAsync(note, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateNoteTextResult(new CreateNoteResponseDTO(note.Id.Value.ToString(), note.Title.Value,
            note.Content.Value));
    }
}